using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public GameObject audioSource2D;

    [Title("Crowd")]
    public AudioClip[] m_CrowdBooing;
    public AudioClip[] m_CrowdLaughing;
    public AudioClip[] m_CrowdCheering;
    public AudioClip[] m_CrowdShocked;
    public AudioClip[] m_CrowdAww;
    

    [Title("Interactions")]
    public AudioClip[] m_KnightInteraction;
    public AudioClip[] m_PrincessInteraction;
    public AudioClip[] m_DragonInteraction;
    public AudioClip[] m_WizardInteraction;

    [Title("TheatreSounds")]
    public AudioClip[] m_CurtainsInteraction;

    [Title("Clics")]
    public AudioClip[] m_Button;
    public AudioClip[] m_Grab;
    public AudioClip[] m_Release;

    private int globalCount = 5;
    private List<AudioSource> _audioSources;

    public enum AudioType
    {
        CrowdBooing,
        CrowdLaughing,
        CrowdCheering,
        KnightInteraction,
        PrincessInteraction,
        DragonInteraction,
        WizardInteraction,
        Button,
        Grab,
        Release,
        Curtains, 
        CrowdShocked,
        Aww
    }
    private void OnEnable()
    {
        MakeAudioSources();
    }

    private void MakeAudioSources()
    {
        _audioSources = new List<AudioSource>();
        for (int i = 0; i < globalCount; i++)
        {
            _audioSources.Add(CreateSource());
        }
    }

    public void PlaySoundGlobal(AudioClip[] _clips, float _pMin, float _pMax, float _vMin, float _vMax)
    {
        int index = Random.Range(0, _clips.Length);
        if (_audioSources == null || _audioSources.Count <= 0 || _clips[index] == null) { return; } // nee oke laat maar dan

        AudioSource sourceUse = null;
        for (int i = 0; i < globalCount; i++)
        {
            AudioSource sourceCheck = _audioSources[i];
            if (sourceCheck != null && !sourceCheck.isPlaying) { sourceUse = sourceCheck; break; }
        }

        if (sourceUse == null) { return; } // kon geen vrije source vinden

        sourceUse.pitch = Random.Range(_pMin, _pMax);
        sourceUse.volume = Random.Range(_vMin, _vMax); // * sound volume setting? (float van 0 tot 1)
        sourceUse.PlayOneShot(_clips[index]);

        // log
        //Debug.Log("play global sound: " + _clip + " || " + Time.time.ToString());
    }

    AudioSource CreateSource(bool _loop = false, bool _playOnAwake = false, AudioClip _clip = null)
    {
        var o = Instantiate(audioSource2D);
        var tr = o.transform;
        tr.SetParent(transform);
        var src = o.GetComponent<AudioSource>();
        src.loop = _loop;
        src.playOnAwake = _playOnAwake;
        src.clip = _clip;
        if (src.playOnAwake) { src.Play(); }
        return src;
    }

    public AudioClip[] GetAudio(AudioType _type)
    {
        switch (_type)
        {
            case AudioType.CrowdBooing: return m_CrowdBooing;
            case AudioType.CrowdLaughing: return m_CrowdLaughing;
            case AudioType.CrowdCheering: return m_CrowdCheering;
            case AudioType.KnightInteraction: return m_KnightInteraction;
            case AudioType.PrincessInteraction: return m_PrincessInteraction;
            case AudioType.DragonInteraction: return m_DragonInteraction;
            case AudioType.WizardInteraction: return m_WizardInteraction;
            case AudioType.Button: return m_Button;
            case AudioType.Grab: return m_Grab;
            case AudioType.Release: return m_Release;
            case AudioType.Curtains: return m_CurtainsInteraction;
            case AudioType.CrowdShocked: return m_CrowdShocked;
            case AudioType.Aww: return m_CrowdAww;
        }
        return null;
    }

    
}
