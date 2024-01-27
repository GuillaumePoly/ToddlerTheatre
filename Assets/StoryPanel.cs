using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanel : MonoBehaviour
{
    [SerializeField]
    private CharacterType _currentCharacterType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCharacter(CharacterType type) => _currentCharacterType = type;
}
