using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public Animator leftCurtain;
    public Animator rightCurtain;
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
        SetCurtains(closed: true);
        yield return new WaitForSeconds(2);
        LowerToddler();
    }

    void SetCurtains(bool closed)
    {
        leftCurtain.SetBool("close", closed);
        rightCurtain.SetBool("close", closed);
    }

    void LowerToddler()
    {
        endingToddler.SetBool("lower", true);
    }
}
