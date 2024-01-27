using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterType type;
    public Animator animator;

    public bool isMoving;

    void Update()
    {
        animator.SetBool("isMoving", isMoving);
    }
}
