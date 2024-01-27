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

        foreach (var panel in panels) panel.isLocked = true;

        _isPlaying = true;
        StartCoroutine(nameof(AnimateMainCharacter));
    }

    IEnumerator AnimateMainCharacter()
    {
        var character = panels.First().currentCharacter;
        yield return new WaitForSeconds(2);

        var secondPanelPos = panels[1].mainCharacterMarker;

        var secondCharacter = panels[1].currentCharacter;

        character.animator.SetBool("isMoving", true);
        foreach (var f in animateCharacterTo(character, secondPanelPos.position)) yield return f;
        character.animator.SetBool("isMoving", false);

        if (Interact(character, secondCharacter))
        {
            yield return new WaitForSeconds(2);

            var thirdPanelPos = panels[2].mainCharacterMarker;
            character.animator.SetBool("isMoving", true);
            foreach (var f in animateCharacterTo(character, thirdPanelPos.position)) yield return f;
            character.animator.SetBool("isMoving", false);

            var thirdCharacter = panels[2].currentCharacter;
            Resolve(character, thirdCharacter);
        }

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
        var mainProp = main.Prop;
        var otherType = other.type;

        var shouldEnd = false;

        switch ((mainProp, otherType))
        {
            // Knight
            case (Prop.sword, CharacterType.Dragon or CharacterType.Princess):
                main.Prop = Prop.key;
                other.Die();
                break;
            case (Prop.sword, CharacterType.Wizard):
                main.Prop = null;
                main.Response = Response.confused;
                shouldEnd = true;
                break;

            // Dragon
            case (Prop.fire, CharacterType.Princess):
                main.Prop = null;
                shouldEnd = true;
                break;
            case (Prop.fire, CharacterType.Knight):
                main.Prop = Prop.sword;
                other.Die();
                break;
            case (Prop.fire, CharacterType.Wizard):
                // Fix this?
                main.Prop = Prop.lightning;
                other.Die();
                break;

            // TODO: Handle sparkle prop cases

            // Wizard
            case (Prop.lightning, CharacterType.Dragon):
                main.Prop = Prop.key;
                other.Die();
                break;
            case (Prop.lightning, CharacterType.Knight):
                main.Prop = null;
                main.Die();
                shouldEnd = true;
                break;
            case (Prop.lightning, CharacterType.Princess):
                other.Die();
                main.Prop = Prop.dress;
                break;

        };

        return !shouldEnd;
    }
    void Resolve(Character main, Character other)
    {
    }
}
