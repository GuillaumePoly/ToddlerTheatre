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

    bool isDead;
    float deathProgress;

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
            case global::Response.confused:
                responseRenderer.sprite = confused;
                break;
            case null:
                responseRenderer.sprite = null;
                break;
        };
    }

    private void SetProp()
    {
        switch (Prop)
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

    public void Die()
    {
        Response = global::Response.death;
        animator.SetBool("isDeath", true);
    }

    //void UpdateDeathProgress()
    //{
    //    deathProgress += Time.deltaTime;
    //    transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.AngleAxis(-90, Vector3.forward), deathProgress);
    //    if (deathProgress >= 1)
    //    {
    //        deathProgress = 1;
    //        isDead = false;
    //    }
    //}
}
