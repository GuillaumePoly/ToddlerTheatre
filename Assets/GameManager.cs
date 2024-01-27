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
    public List<Transform> mainCharacterMarkers;
    public GameObject endingPrefab;

    void Start()
    {
    }

    void Update()
    {
    }

    public void PlayScene()
    {
        StartCoroutine("AnimateMainCharacter");
    }

    IEnumerator AnimateMainCharacter()
    {
        var character = panels.First().GetComponentInChildren<Character>();
        yield return new WaitForSeconds(2);

        var secondPanelPos = mainCharacterMarkers[1];

        foreach (var f in animateCharacterTo(character, secondPanelPos.position)) yield return f;

        yield return new WaitForSeconds(2);

        var thirdPanelPos = mainCharacterMarkers[2];
        foreach (var f in animateCharacterTo(character, thirdPanelPos.position)) yield return f;

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
        var ending = Instantiate(endingPrefab, new Vector3(0, 0, 10), Quaternion.identity, Camera.main.transform);
        var tmp = ending.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = description;
    }
}
