using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterType type;
    public Animator animator;

    private Response? _response;
    private Prop? _prop;

    public Sprite heart;
    public Sprite heartbreak;
    public Sprite skull;
    public Sprite swooning;
    public Sprite confused;
    public Sprite muscle;
    public Sprite yum;
    public Sprite teacher;
    public Sprite books;
    public Sprite hatchling;
    public Sprite chef;
    public Sprite husband;
    public Sprite wife;
    public Sprite confounded;
    public Sprite rollercoaster;
    public Sprite balloon;

    public Sprite sword;
    public Sprite charm;
    public Sprite dress;
    public Sprite fire;
    public Sprite lightning;
    public Sprite key;
    public Sprite magic;
    public Sprite banana;
    public Sprite wizard;
    public Sprite dragon;
    public Sprite knight;

    public SpriteRenderer characterRenderer;
    public SpriteRenderer responseRenderer;
    public SpriteRenderer propRenderer;

    public GameObject m_PropSwitchVFX;
    public bool m_DisableVFX;

    public GameObject frogHead;

    public Response? Response
    {
        get => _response; set
        {
            if (_response == value) return;
            _response = value;
            SetResponse();
        }
    }

    public Prop? Prop
    {
        get => _prop; set
        {
            if (_prop == value) return;
            _prop = value;
            SetProp();
        }
    }

    public void MakeFrog()
    {
        frogHead.SetActive(true);
    }

    void Start()
    {
        SetStartProp();
    }

    public void SetStartProp()
    {
        Prop = type switch
        {
            CharacterType.Knight => global::Prop.sword,
            CharacterType.Dragon => global::Prop.fire,
            CharacterType.Princess => global::Prop.sparkle,
            CharacterType.Wizard => global::Prop.lightning,
            _ => throw new NotImplementedException(),
        };
    }

    private void SetResponse()
    {
        responseRenderer.sprite = Response switch
        {
            global::Response.love => heart,
            global::Response.swooning => swooning,
            global::Response.heartbreak => heartbreak,
            global::Response.death => skull,
            global::Response.confused => confused,
            global::Response.strength => muscle,
            global::Response.yum => yum,
            global::Response.teacher => teacher,
            global::Response.book => books,
            global::Response.hatchling => hatchling,
            global::Response.chef => chef,
            global::Response.husband => husband,
            global::Response.wife => wife,
            global::Response.confounded => confounded,
            global::Response.magic => magic,
            global::Response.rollercoaster => rollercoaster,
            global::Response.balloon => balloon,
            _ => null,
        };
    }

    private void SetProp()
    {
        if (!m_DisableVFX)
        {
            var vfx = Instantiate(m_PropSwitchVFX);
            vfx.transform.position = propRenderer.transform.position;
        }

        propRenderer.sprite = Prop switch
        {
            global::Prop.lightning => lightning,
            global::Prop.sword => sword,
            global::Prop.sparkle => charm,
            global::Prop.fire => fire,
            global::Prop.key => key,
            global::Prop.dress => dress,
            global::Prop.magic => magic,
            global::Prop.banana => banana,

            global::Prop.knight => knight,
            global::Prop.wizard => wizard,
            global::Prop.dragon => dragon,
            _ => null,
        };
    }

    public void Die(bool setResponse = true)
    {
        if (setResponse)
        {
            Response = null;
        }
        Prop = null;
        animator.SetBool("isDeath", true);
    }
}
