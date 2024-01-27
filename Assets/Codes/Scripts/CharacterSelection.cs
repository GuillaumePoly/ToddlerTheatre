using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameManager gameManager;
    public List<Transform> markers;

    private Camera cam;
    private RaycastHit2D hit;
    [SerializeField]
    private Transform selectedCharacter;
    private void OnEnable()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (InputReceiver.m_Clicked)
        {
            hit = Physics2D.Raycast(InputReceiver.m_WorldPointerPos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Toddler"));
            if (hit.transform != null)
            {
                selectedCharacter = hit.transform;
            }
        }
        else
        {
            selectedCharacter = null;
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
