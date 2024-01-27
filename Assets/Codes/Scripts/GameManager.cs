using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<StoryPanel> panels;
    public List<Transform> mainCharacterMarkers;
    
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
    }

    IEnumerable animateCharacterTo(Character character, Vector3 target)
    {
        var elapsedTime = 0f;
        var waitTime = 3f;
        while (elapsedTime < waitTime)
        {
            character.transform.position = Vector3.Lerp(character.transform.position, target, elapsedTime / waitTime);
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
    }
}
