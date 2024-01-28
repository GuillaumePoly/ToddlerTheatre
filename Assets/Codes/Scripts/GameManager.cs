using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<StoryPanel> panels;
    public GameObject endingPrefab;

    public AudioManager audioManager;

    public Animator leftCurtain;
    public Animator rightCurtain;

    public GameObject m_PlayButton;

    public bool isReadyToPlay => panels.All(p => !p.IsFree);

    private bool _isPlaying;

    public PostProcessVolume pp;
    public float m_MaxVignette;

    private bool startVignette;
    private float vignetteDelta;

    void Update()
    {
        if (isReadyToPlay)
        {
            startVignette = true;
        }

        if (startVignette)
        {
            Vignette();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void Vignette()
    {
        
        if (vignetteDelta <= m_MaxVignette)
        {
            vignetteDelta += Time.deltaTime;
            pp.profile.GetSetting<Vignette>().intensity.value = vignetteDelta;
        }
        else
        {
            startVignette = false;
            vignetteDelta = m_MaxVignette;
        }
        
    }
    public void TryPlay()
    {
        if (isReadyToPlay)
        {
            PlayScene();
        }
    }

    public void PlayScene()
    {
        if (_isPlaying) return;
        m_PlayButton.SetActive(false);

        foreach (var panel in panels) panel.isLocked = true;

        _isPlaying = true;
        StartCoroutine(nameof(AnimateMainCharacter));
    }
    IEnumerator StartVignette()
    {

        float t = 0;
        if (t <= m_MaxVignette)
        {
            t += Time.deltaTime;
        }
        pp.GetSetting<Vignette>().intensity.value = t;
        yield return null;
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

        if (!(character.type == CharacterType.Dragon && secondCharacter.type == CharacterType.Princess))
        {
            character.animator.SetBool("IsAttacking", true);
        }
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

        yield return new WaitForSeconds(3);

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
            AudienceResponse.boo => AudioManager.AudioType.CrowdBooing,
            AudienceResponse.horrifying => AudioManager.AudioType.CrowdShocked,
            AudienceResponse.awww => AudioManager.AudioType.Aww,
        };
        audioManager.PlaySoundGlobal(audioManager.GetAudio(audioType), 0.9f, 1.1f, 0.8f, 1f);

        SetCurtains(closed: true);

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
            case (Prop.sword, CharacterType.Dragon):
                main.Prop = Prop.key;
                other.Die();
                break;
            case (Prop.sword, CharacterType.Princess):
                main.Prop = Prop.dress;
                other.Die();
                break;

            case (Prop.sword, CharacterType.Wizard):
                main.Prop = Prop.banana;
                main.Response = Response.confused;
                other.Response = Response.magic;
                endStory = new Story()
                {
                    audience = AudienceResponse.boo,
                    title = "Play Again",
                };
                break;

            // Dragon
            case (Prop.fire, CharacterType.Knight):
                main.Prop = Prop.sword;
                other.Die();
                break;
            case (Prop.fire, CharacterType.Princess):
                main.Prop = null;
                main.Response = Response.confounded;
                endStory = new Story()
                {
                    audience = AudienceResponse.boo,
                    title = "PLAY AGAIN",
                };
                break;
            case (Prop.fire, CharacterType.Wizard):
                main.Prop = Prop.magic;
                other.Die();
                break;

            // Princess
            case (Prop.sparkle, CharacterType.Dragon):
                StartCoroutine(nameof(Charm), new CharmArguments
                {
                    princess = main,
                    other = other,
                    otherProp = Prop.dragon,
                });
                break;
            case (Prop.sparkle, CharacterType.Knight):
                StartCoroutine(nameof(Charm), new CharmArguments
                {
                    princess = main,
                    other = other,
                    otherProp = Prop.knight,
                });
                break;

            case (Prop.sparkle, CharacterType.Wizard):
                main.MakeFrog();
                other.Response = Response.magic;
                endStory = new Story()
                {
                    audience = AudienceResponse.boo,
                    title = "Play Again",
                };
                break;

            // Wizard
            case (Prop.lightning, CharacterType.Dragon):
                main.Prop = Prop.key;
                other.Die();
                break;
            case (Prop.lightning, CharacterType.Knight):
                main.animator.SetBool("IsAttacking", false);
                main.Prop = null;
                main.Die();
                other.Response = Response.strength;
                endStory = new Story()
                {
                    audience = AudienceResponse.boo,
                    title = "Play Again",
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
                mainResponse = Response.husband;
                otherResponse = Response.wife;
                story = new Story
                {
                    audience = AudienceResponse.clap,
                    title = "The Original Story",
                };
                break;
            case (CharacterType.Knight, Prop.key, CharacterType.Wizard):
                mainProp = null;
                mainResponse = Response.husband;
                otherResponse = Response.husband;
                story = new Story
                {
                    audience = AudienceResponse.awww,
                    title = "Magic Marriage",
                };
                break;
            case (CharacterType.Knight, Prop.dress, CharacterType.Dragon):
                mainProp = null;
                mainResponse = Response.love;
                otherResponse = Response.love;
                story = new Story
                {
                    audience = AudienceResponse.awww,
                    title = "Flames of Passion",
                };
                break;
            case (CharacterType.Knight, Prop.dress, CharacterType.Wizard):
                mainProp = null;
                mainResponse = Response.husband;
                otherResponse = Response.husband;
                story = new Story
                {
                    audience = AudienceResponse.awww,
                    title = "Magic Marriage",
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
                    title = "Good Boy",
                };
                break;
            case (CharacterType.Dragon, Prop.sword, CharacterType.Wizard):
                mainProp = null;
                mainResponse = Response.hatchling;
                otherResponse = Response.love;
                story = new Story
                {
                    audience = AudienceResponse.laughing,
                    title = "Good Girl",
                };
                break;
            case (CharacterType.Dragon, Prop.magic, CharacterType.Princess):
                mainProp = null;
                mainResponse = Response.chef;
                otherResponse = Response.yum;
                story = new Story
                {
                    audience = AudienceResponse.laughing,
                    title = "The Scaly Chef",
                };
                break;
            case (CharacterType.Dragon, Prop.magic, CharacterType.Knight):
                mainProp = null;
                mainResponse = Response.strength;
                otherDies = true;
                main.animator.SetBool("IsAttacking", true);
                story = new Story
                {
                    audience = AudienceResponse.horrifying,
                    title = "Fire Brigade",
                };
                break;

            // Princess stories
            case (CharacterType.Princess, Prop.knight, CharacterType.Dragon):
                mainProp = Prop.knight;
                mainResponse = Response.love;
                otherResponse = Response.heartbreak;
                otherDies = true;
                story = new Story
                {
                    audience = AudienceResponse.horrifying,
                    title = "Heartbroken",
                };
                break;
            case (CharacterType.Princess, Prop.knight, CharacterType.Wizard):
                mainProp = Prop.knight;
                mainResponse = Response.love;
                otherResponse = Response.love;
                story = new Story
                {
                    audience = AudienceResponse.clap,
                    title = "Everybody Happy",
                };
                break;
            case (CharacterType.Princess, Prop.dragon, CharacterType.Knight):
                mainProp = null;
                mainResponse = Response.love;
                otherResponse = Response.love;
                story = new Story
                {
                    audience = AudienceResponse.clap,
                    title = "Scaly Happy People",
                };
                break;
            case (CharacterType.Princess, Prop.dragon, CharacterType.Wizard):
                mainProp = null;
                mainResponse = Response.chef;
                otherResponse = Response.chef;
                story = new Story
                {
                    audience = AudienceResponse.clap,
                    title = "Fiery Feast",
                };
                break;


            // Wizard stories
            case (CharacterType.Wizard, Prop.key, CharacterType.Princess):
                mainProp = null;
                mainResponse = Response.teacher;
                otherResponse = Response.book;
                story = new Story
                {
                    audience = AudienceResponse.clap,
                    title = "The Apprentice",
                };
                break;
            case (CharacterType.Wizard, Prop.key, CharacterType.Knight):
                mainProp = null;
                mainResponse = Response.rollercoaster;
                otherResponse = Response.balloon;
                story = new Story
                {
                    audience = AudienceResponse.clap,
                    title = "Rollercastle",
                };
                break;
            case (CharacterType.Wizard, Prop.dress, CharacterType.Knight):
                mainProp = null;
                mainResponse = Response.husband;
                otherResponse = Response.husband;
                story = new Story
                {
                    audience = AudienceResponse.awww,
                    title = "Mrs. Doubtfire",
                };
                break;
            case (CharacterType.Wizard, Prop.dress, CharacterType.Dragon):
                mainProp = null;
                mainResponse = Response.love;
                otherResponse = Response.hatchling;
                story = new Story
                {
                    audience = AudienceResponse.laughing,
                    title = "Good Girl",
                };
                break;
        };

        main.Prop = mainProp;
        main.Response = mainResponse;
        if (otherDies)
        {
            if (otherResponse != null)
            {
                other.Response = otherResponse;
            }
            other.Die(setResponse: otherResponse == null);
        }
        else
        {
            other.Response = otherResponse;
        }

        return story;
    }

    void SetCurtains(bool closed)
    {
        leftCurtain.SetBool("close", closed);
        rightCurtain.SetBool("close", closed);
    }

    IEnumerator Charm(object obj)
    {
        var arg = obj as CharmArguments;

        yield return new WaitForSeconds(0.8f);
        arg.princess.Prop = arg.otherProp;
        Destroy(arg.other.gameObject);
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

class CharmArguments
{
    public Character princess;
    public Character other;
    public Prop otherProp;
}