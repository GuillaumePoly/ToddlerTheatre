using System;
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

    public AudioManager audioManager;

    public bool isReadyToPlay => panels.All(p => !p.IsFree);

    private bool _isPlaying;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }


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

        Story ending;

        character.animator.SetBool("IsAttacking", true);
        ending = Interact(character, secondCharacter);

        if (ending == null)
        {
            yield return new WaitForSeconds(1);
            character.animator.SetBool("IsAttacking", false);
            yield return new WaitForSeconds(2);
            var thirdPanelPos = panels[2].mainCharacterMarker;
            character.animator.SetBool("isMoving", true);
            foreach (var f in animateCharacterTo(character, thirdPanelPos.position)) yield return f;
            character.animator.SetBool("isMoving", false);

            var thirdCharacter = panels[2].currentCharacter;
            ending = Resolve(character, thirdCharacter);
        }

        yield return new WaitForSeconds(2);

        AddEnding(ending);
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

    void AddEnding(Story story)
    {
        var audioType = story.audience switch
        {
            AudienceResponse.clap => AudioManager.AudioType.CrowdCheering,
            AudienceResponse.laughing => AudioManager.AudioType.CrowdLaughing,
            AudienceResponse.boo => AudioManager.AudioType.CrowdLaughing,
            _ => AudioManager.AudioType.CrowdCheering,
            //AudienceResponse.awww => throw new NotImplementedException(),
            //AudienceResponse.horrifying => throw new NotImplementedException(),
        };
        audioManager.PlaySoundGlobal(audioManager.GetAudio(audioType), 0.9f, 1.1f, 0.8f, 1f);

        var ending = Instantiate(endingPrefab, new Vector3(0, 0, 10), Quaternion.identity);
        var tmp = ending.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = story.title;
    }

    Story Interact(Character main, Character other)
    {
        Story endStory = null;

        switch ((mainProp: main.Prop, otherType: other.type))
        {
            // Knight
            case (Prop.sword, CharacterType.Dragon or CharacterType.Princess):
                main.Prop = Prop.key;
                other.Die();
                break;
            case (Prop.sword, CharacterType.Wizard):
                main.Prop = null;
                main.Response = Response.confused;
                endStory = new Story()
                {
                    audience = AudienceResponse.boo
                };
                break;

            // Dragon
            case (Prop.fire, CharacterType.Princess):
                main.Prop = null;
                endStory = new Story()
                {
                    audience = AudienceResponse.boo
                };
                break;
            case (Prop.fire, CharacterType.Knight):
                main.Prop = Prop.sword;
                other.Die();
                break;
            case (Prop.fire, CharacterType.Wizard):
                // Fix this?
                main.Prop = Prop.magic;
                other.Die();
                break;

            // TODO: Handle sparkle prop cases
            case (Prop.dress, CharacterType.Dragon):
                break;

            case (Prop.dress, CharacterType.Wizard):
                endStory = new Story()
                {
                    audience = AudienceResponse.boo
                };
                break;

            // Wizard
            case (Prop.lightning, CharacterType.Dragon):
                main.Prop = Prop.key;
                other.Die();
                break;
            case (Prop.lightning, CharacterType.Knight):
                main.Prop = null;
                main.Die();
                endStory = new Story()
                {
                    audience = AudienceResponse.boo
                };
                break;
            case (Prop.lightning, CharacterType.Princess):
                other.Die();
                main.Prop = Prop.dress;
                break;

        };

        return endStory;
    }
    Story Resolve(Character main, Character other)
    {
        Prop? mainProp = null;
        Response? mainResponse = null;
        Response? otherResponse = null;
        bool otherDies = false;

        Story story = null;

        switch (main.type, main.Prop, other.type)
        {
            // Knight Stories
            case (CharacterType.Knight, Prop.key, CharacterType.Princess):
                mainProp = null;
                mainResponse = Response.love;
                otherResponse = Response.love;
                story = new Story
                {
                    audience = AudienceResponse.awww,
                };
                break;
            case (CharacterType.Knight, Prop.key, CharacterType.Wizard):
                mainProp = null;
                mainResponse = Response.book;
                otherResponse = Response.teacher;
                story = new Story
                {
                    audience = AudienceResponse.clap,
                };
                break;
            case (CharacterType.Knight, Prop.dress, CharacterType.Dragon or CharacterType.Wizard):
                mainProp = null;
                mainResponse = Response.love;
                otherResponse = Response.love;
                story = new Story
                {
                    audience = AudienceResponse.awww,
                };
                break;

            // Dragon stories
            case (CharacterType.Dragon, Prop.sword, CharacterType.Princess):
                mainProp = null;
                mainResponse = Response.hatchling;
                otherResponse = Response.love;
                story = new Story
                {
                    audience = AudienceResponse.laughing,
                };
                break;
            case (CharacterType.Dragon, Prop.sword, CharacterType.Wizard):
                mainProp = null;
                mainResponse = Response.love;
                otherResponse = Response.love;
                story = new Story
                {
                    audience = AudienceResponse.laughing,
                };
                break;
            case (CharacterType.Dragon, Prop.magic, CharacterType.Princess):
                mainProp = null;
                mainResponse = Response.chef;
                otherResponse = Response.yum;
                story = new Story
                {
                    audience = AudienceResponse.laughing,
                };
                break;
            case (CharacterType.Dragon, Prop.magic, CharacterType.Knight):
                mainProp = null;
                mainResponse = Response.strength;
                otherDies = true;
                story = new Story
                {
                    audience = AudienceResponse.horrifying,
                };
                break;

            // Princess stories
            // TODO

            // Wizard stories
            case (CharacterType.Wizard, Prop.key, CharacterType.Princess):
                mainProp = null;
                mainResponse = Response.teacher;
                otherResponse = Response.book;
                story = new Story
                {
                    audience = AudienceResponse.clap,
                };
                break;


        };

        main.Prop = mainProp;
        main.Response = mainResponse;
        if (otherDies)
        {
            other.Die();
        }
        else
        {
            other.Response = otherResponse;
        }

        return story;
    }
}

public class Story
{
    public string title;
    public AudienceResponse audience;
}

public enum AudienceResponse
{
    clap,
    laughing,
    awww,
    boo,
    horrifying,
}