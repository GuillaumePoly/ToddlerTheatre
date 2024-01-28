using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class CharacterSelection : MonoBehaviour
{
    public GameManager gameManager;
    public List<Transform> markers;
    public AudioManager m_AudioManager;

    private Camera cam;
    private RaycastHit2D hit;
    [SerializeField]
    private Transform selectedCharacter;

    bool playSound = true;

    private void OnEnable()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (InputReceiver.m_PressedThisFrame)
        {
            hit = Physics2D.Raycast(InputReceiver.m_WorldPointerPos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Toddler"));

            if (playSound)
            {
                m_AudioManager.PlaySoundGlobal(m_AudioManager.GetAudio(AudioManager.AudioType.Grab), 0.8f, 1f, 0.9f, 1.1f);
                playSound = false;
            }

            if (hit.transform != null)
            {
                selectedCharacter = hit.transform;
            }
        }
        else if (InputReceiver.m_ReleasedThisFrame)
        {
            selectedCharacter = null;
            playSound = true;
        }

        if (selectedCharacter != null)
        {
            selectedCharacter.transform.position = InputReceiver.m_WorldPointerPos;
        }
    }
}
