using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public GameManager gameManager;
    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isReadyToPlay && InputReceiver.m_Clicked)
        {
            var hit = Physics2D.Raycast(InputReceiver.m_WorldPointerPos, Vector2.zero, Mathf.Infinity, mask);
            if (hit.transform != null)
            {
                gameManager.PlayScene();
            }
        }
    }
}
