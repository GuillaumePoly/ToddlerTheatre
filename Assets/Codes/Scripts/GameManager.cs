using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmplifyShaderEditor;
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

    void Start()
    {
    }

    void Update()
    {
    }

    public void PlayScene()
    {
        if (_isPlaying) return;

        _isPlaying = true;
        StartCoroutine(nameof(AnimateMainCharacter));
    }

    IEnumerator AnimateMainCharacter()
    {
        var mainCharacter = panels.First().currentCharacter;
        yield return new WaitForSeconds(2);

        var secondPanelPos = panels[1].mainCharacterMarker;

        mainCharacter.isMoving = true;
        foreach (var f in animateCharacterTo(mainCharacter, secondPanelPos.position)) yield return f;
        mainCharacter.isMoving = false;

        var secondCharacter = panels[1].currentCharacter;
        var continueToNextPanel = Interact(mainCharacter, secondCharacter);

        if (continueToNextPanel)
        {
            yield return new WaitForSeconds(2);

            var thirdPanelPos = panels[2].mainCharacterMarker;
            mainCharacter.isMoving = true;
            foreach (var f in animateCharacterTo(mainCharacter, thirdPanelPos.position)) yield return f;
            mainCharacter.isMoving = false;

            var thirdCharacter = panels[2].currentCharacter;
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
        var tmp = ending.GetComponentInChildren<TextMeshPro>();
        tmp.text = description;
    }

    bool Interact(Character main, Character other)
    {
        main.Response = Response.love;
        other.Response = Response.heartbreak;

        main.prop = Prop.fire;
        other.prop = Prop.key;

        main.SetProp();
        other.SetProp();

        return true;
    }
}
