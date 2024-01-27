using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
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

    private bool canPlaySound = true;
    private void OnEnable()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (InputReceiver.m_Clicked)
        {
            hit = Physics2D.Raycast(InputReceiver.m_WorldPointerPos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Toddler"));
            if (canPlaySound)
            {
                m_AudioManager.PlaySoundGlobal(m_AudioManager.GetAudio(AudioManager.AudioType.Grab),0.8f, 1f, 0.9f, 1.1f);
                canPlaySound = false;
            }
                
            if (hit.transform != null)
            {
                selectedCharacter = hit.transform;
            }
        }
        else
        {
            selectedCharacter = null;
            canPlaySound = true;
        }

        if (selectedCharacter != null)
        {
            selectedCharacter.transform.position = InputReceiver.m_WorldPointerPos;
        }


        // if (hit.collider.CompareTag("Player"))
        // {
        //     selectedCharacter = hit.transform;
        //     selectedCharacter.transform.position = InputReceiver.m_WorldPointerPos;
        // }
        // else
        // {
        //     selectedCharacter = null;
        //     return;
        // }

        // var freeMarker = markers.FirstOrDefault(m => m.childCount == 0);
        //
        // if (freeMarker != null)
        // {
        //     var characterScript = selectedCharacter.GetComponent<Character>();
        //     freeMarker.GetComponentInParent<StoryPanel>().SetCharacter(characterScript.type);
        //     selectedCharacter.parent = freeMarker;
        //     selectedCharacter.position = freeMarker.position;
        // }
    }
}
