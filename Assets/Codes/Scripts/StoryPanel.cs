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

    public bool isLocked;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _hoverCharacter = other.GetComponent<Character>();
        }
    }

    void Update()
    {
        if (isLocked) return;

        if (InputReceiver.m_ReleasedThisFrame)
        {
            if (_hoverCharacter != null && IsFree)
            {
                _currentCharacter = _hoverCharacter;
                // Snap the character to the right position.
                _currentCharacter.transform.position = isMain ? mainCharacterMarker.position : panelCharacterMarker.position;

                _currentCharacter.characterRenderer.flipX = isMain ? false : true;

                if (!isMain)
                {
                    _currentCharacter.m_DisableVFX = true;
                    _currentCharacter.Prop = null;
                    _currentCharacter.m_DisableVFX = false;
                }
                else
                {
                    _currentCharacter.SetStartProp();
                }
            }

            _hoverCharacter = null;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Character>() == currentCharacter)
            {
                _currentCharacter = null;
            }

            _hoverCharacter = null;
        }
    }
}
