using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFire : MonoBehaviour
{
    public Animator m_Animator;

    public void Fire()
    {
        m_Animator.SetBool("Fire", true);
    }

    public void StopFire()
    {
        m_Animator.SetBool("Fire", false);
    }
}
