using UnityEngine;
using UnityEngine.Assertions;


// “G‚ð“|‚·‚Æ—Ž‚Æ‚·ƒAƒCƒeƒ€
[RequireComponent(typeof(CpMoveComponent))]
public class CpDropItem : CpActorBase, ICpGameplayEffectSender
{
    [SerializeField] CpPlayerItemGameplayEffect PlayerItemGameplayEffect = null;
    public CpMoveComponent _moveComponent = null;
    public FCpMoveParamPhysical _moveParam;
    protected override void Awake()
    {
        base.Awake();
        _moveComponent = GetComponent<CpMoveComponent>();
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
            rb2d.AddForce(new Vector2(100f, 30f), ForceMode2D.Impulse);
        }
    }

    // ICpActorForwardInterface

    public override float GetForwardDegree()
    {
        return _initialDegree;
    }

    // end of ICpActorForwardInterface

    public void RequestStartBehavior(in FCpItemDropRandomScatterRequestParam reqParam)
    {
        transform.SetPosition(reqParam.InitialPosition);
        _initialDegree = reqParam.InitialDegree;
        // transform.SetRotation(reqParam.InitialDegree);
        _moveComponent.RequestStart(reqParam.MoveParamPhysical);
    }

    // ICpGameplayEffectSender
    public CpGameplayEffectBase GetGameplayEffect()
    {
        return PlayerItemGameplayEffect.Effect;
    }

    // end of ICpGameplayEffectSender

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        ICpGameplayEffectReciever geReciever = collision.GetComponent<ICpGameplayEffectReciever>();
        geReciever?.OnGameplayEffectRecieve(this);

        if (collision.gameObject.layer == CpLayer.ItemPickup)
        {
            Destroy(this.gameObject);
        }
    }

    float _initialDegree = 0f;
}
