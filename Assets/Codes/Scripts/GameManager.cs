using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<StoryPanel> panels;
    public GameObject endingPrefab;

    public bool isReadyToPlay => panels.All(p => !p.IsFree);

    private bool _isPlaying;


    public void PlayScene()
    {
        if (_isPlaying) return;

        _isPlaying = true;
        StartCoroutine(nameof(AnimateMainCharacter));
    }

    IEnumerator AnimateMainCharacter()
    {
        var character = panels.First().currentCharacter;
        yield return new WaitForSeconds(2);

        var secondPanelPos = panels[1].mainCharacterMarker;

        character.animator.SetBool("isMoving", true);
        foreach (var f in animateCharacterTo(character, secondPanelPos.position)) yield return f;
        character.animator.SetBool("isMoving", false);

        yield return new WaitForSeconds(2);

        var thirdPanelPos = panels[2].mainCharacterMarker;
        character.animator.SetBool("isMoving", true);
        foreach (var f in animateCharacterTo(character, thirdPanelPos.position)) yield return f;
        character.animator.SetBool("isMoving", false);

        yield return new WaitForSeconds(2);

        AddEnding("Happy Family");
    }
    
    
    IEnumerable animateCharacterTo(Character character, Vector3 target)
    {
        var initialPos = character.transform.position;

        var elapsedTime = 0f;
        var waitTime = 3f;
        while (elapsedTime < waitTime)
        {
            character.transform.position = Vector3.Lerp(initialPos, target, elapsedTime / waitTime);
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
    }

    void AddEnding(string description)
    {
        var ending = Instantiate(endingPrefab, new Vector3(0, 0, 10), Quaternion.identity);
        var tmp = ending.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = description;
    }

    bool Interact(Character main, Character other)
    {
        if (main.Prop == Prop.sword)
        {
            main.Response = Response.love;
            other.Die();
        }
        else
        {
            main.Response = Response.love;
            other.Response = Response.heartbreak;
        }

        main.Prop = Prop.fire;
        other.Prop = null;

        return true;
    }
}