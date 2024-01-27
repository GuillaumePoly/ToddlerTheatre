using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanel : MonoBehaviour
{
    [SerializeField]
    private Character _hoverCharacter;

    [SerializeField]
    private Character _currentCharacter;

    public Transform panelCharacterMarker;
    public Transform mainCharacterMarker;

    public bool isMain;

    public bool IsFree => _currentCharacter == null;

    public Character currentCharacter => _currentCharacter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _hoverCharacter = other.GetComponent<Character>();
        }
    }

    void Update()
    {
        if (InputReceiver.m_ClickedPrevious && !InputReceiver.m_Clicked)
        {
            if (_hoverCharacter != null && IsFree)
            {
                _currentCharacter = _hoverCharacter;
                // Snap the character to the right position.
                _currentCharacter.transform.position = isMain ? mainCharacterMarker.position : panelCharacterMarker.position;
                _currentCharacter.GetComponentInChildren<SpriteRenderer>().flipX = !isMain;
            }

            _hoverCharacter = null;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _hoverCharacter = null;
        }
    }
}
