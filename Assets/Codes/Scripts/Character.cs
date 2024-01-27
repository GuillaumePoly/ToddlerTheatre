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

    public Sprite sword;
    public Sprite charm;
    public Sprite dress;
    public Sprite fire;
    public Sprite lightning;
    public Sprite key;

    public SpriteRenderer characterRenderer;
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

    public Prop? Prop
    {
        get => _prop; set
        {
            _prop = value;
            SetProp();
        }
    }

    void Start()
    {
        switch (type)
        {
            case CharacterType.Knight:
                Prop = global::Prop.sword;
                break;
            case CharacterType.Dragon:
                Prop = global::Prop.fire;
                break;
            case CharacterType.Princess:
                Prop = global::Prop.dress;
                break;
        }
    }

    void Update()
    {
        animator.SetBool("isMoving", isMoving);

        //if (isDead)
        //{
        //    //UpdateDeathProgress();
        //}
    }

    private void SetResponse()
    {
        responseRenderer.sprite = Response switch
        {
            global::Response.love => heart,
            global::Response.swooning => swooning,
            global::Response.disgust => null,
            global::Response.heartbreak => heartbreak,
            global::Response.death => skull,
            global::Response.confused => confused,
            _ => null,
        };
    }

    private void SetProp()
    {
        propRenderer.sprite = Prop switch
        {
            global::Prop.lightning => lightning,
            global::Prop.sword => sword,
            global::Prop.fire => fire,
            global::Prop.key => key,
            global::Prop.dress => dress,
            _ => null,
        };
    }

    public void Die()
    {
        Response = global::Response.death;
        animator.SetBool("isDeath", true);
    }
}
