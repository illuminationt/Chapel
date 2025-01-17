using UnityEngine;
using UnityEngine.Assertions;


// “G‚ð“|‚·‚Æ—Ž‚Æ‚·ƒAƒCƒeƒ€
[RequireComponent(typeof(CpMoveComponent))]
public class CpDropItem : CpActorBase, ICpGameplayEffectSender
{
    [SerializeField] CpPlayerItemGameplayEffect PlayerItemGameplayEffect = null;
    CpMoveComponent _moveComponent = null;
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

    // CpActorBase interface
    public override ECpMoverUpdateType GetMoverUpdateType() { return ECpMoverUpdateType.UpdateFunction; }

    // end of CpActorBase interface

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
        if (collision.gameObject.layer == CpLayer.ItemPickup)
        {
            ICpGameplayEffectReciever geReciever = collision.GetComponent<ICpGameplayEffectReciever>();
            geReciever?.OnGameplayEffectRecieve(this);
            Destroy(this.gameObject);
        }
    }

    float _initialDegree = 0f;
}
