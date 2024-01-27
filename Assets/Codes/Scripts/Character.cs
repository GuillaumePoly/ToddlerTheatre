using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Properties;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterType type;
    public Animator animator;

    private Response? _response;
    public Prop prop;

    public Sprite heart;
    public Sprite heartbreak;
    public Sprite skull;

    public Sprite sword;
    public Sprite charm;
    public Sprite dress;
    public Sprite fire;
    public Sprite lightning;
    public Sprite key;

    public SpriteRenderer responseRenderer;
    public SpriteRenderer propRenderer;


    public bool isMoving;

    public Response? Response
    {
        get => _response; set
        {
            _response = value;
            SetResponse();
        }
    }

    void Update()
    {
        animator.SetBool("isMoving", isMoving);
    }

    private void SetResponse()
    {
        switch (Response)
        {
            case global::Response.love:
                responseRenderer.sprite = heart;
                break;
            case global::Response.disgust:
                responseRenderer.sprite = null;
                break;
            case global::Response.heartbreak:
                responseRenderer.sprite = heartbreak;
                break;
            case global::Response.death:
                responseRenderer.sprite = skull;
                break;
            case null:
                responseRenderer.sprite = null;
                break;
        };
    }

    public void SetProp()
    {
        switch (prop)
        {
            case global::Prop.lightning:
                propRenderer.sprite = lightning;
                break;
            case global::Prop.sword:
                propRenderer.sprite = sword;
                break;
            case global::Prop.fire:
                propRenderer.sprite = fire;
                break;
            case global::Prop.key:
                propRenderer.sprite = key;
                break;
            case global::Prop.dress:
                propRenderer.sprite = dress;
                break;
        }
    }
}
