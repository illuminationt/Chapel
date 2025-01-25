using ImGuiNET;
using UnityEngine;
using UnityEngine.Assertions;


// “G‚ð“|‚·‚Æ—Ž‚Æ‚·ƒAƒCƒeƒ€
[RequireComponent(typeof(CpMoveComponent))]
public class CpDropItem : CpActorBase,
    ICpGameplayEffectSender,
    ICpAbsorbTarget
{
    [SerializeField] CpPlayerItemGameplayEffect PlayerItemGameplayEffect = null;
    [SerializeField] CpMoveParamScriptableObject AbsorbMoveParamSettings = null;
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
    public override void OnActivated()
    {
        base.OnActivated();
        AttachCurrentRoom();
    }
    // end of CpActorBase interface

    // ICpPoolable
    public override void ResetOnRelease()
    {
        base.ResetOnRelease();
        bAlreadyStartAbsorb = false;
    }
    // end of ICpPoolable

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
        _moveComponent.RequestStart(reqParam.MoveParamPhysical);
    }

    // ICpGameplayEffectSender
    public CpGameplayEffectBase GetGameplayEffect()
    {
        return PlayerItemGameplayEffect.Effect;
    }
    // end of ICpGameplayEffectSender

    // ICpAbsorbTarget

    public bool CanAbsorb(ICpAbsorbable absorbable)
    {
        if (bAlreadyStartAbsorb)
        {
            return false;
        }
        return true;
    }
    public void StartAbsorb(ICpAbsorbable absorbable)
    {
        _moveComponent.RequestStopAll();
        bAlreadyStartAbsorb = true;
        _moveComponent.RequestStart(AbsorbMoveParamSettings.MoveParam);
    }

    // end of ICpAbsorbTarget

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
    bool bAlreadyStartAbsorb = false;
#if CP_DEBUG

    public override void DrawImGui()
    {
        base.DrawImGui();

        if (ImGui.TreeNode("Move Component"))
        {
            _moveComponent.DrawImGui();
            ImGui.TreePop();
        }
    }
#endif
}
