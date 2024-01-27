using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public Animator endingToddler;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(PlayEndingAnimation));
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator PlayEndingAnimation()
    {
        yield return new WaitForSeconds(2);
        LowerToddler();
    }


    void LowerToddler()
    {
        endingToddler.SetBool("lower", true);
    }
}
