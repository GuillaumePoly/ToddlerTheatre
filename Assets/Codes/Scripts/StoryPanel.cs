using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanel : MonoBehaviour
{
    [SerializeField]
    private Character _currentCharacter;
    [SerializeField] 
    private CharacterType _currentCharacterType;

    public bool isMain;
    public void SetCharacter(CharacterType type) => _currentCharacterType = type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _currentCharacter==null)
        {
            _currentCharacter = other.GetComponent<Character>();
            _currentCharacterType = _currentCharacter.type;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _currentCharacter!=null)
        {
            _currentCharacter = null;
        }
    }
}
