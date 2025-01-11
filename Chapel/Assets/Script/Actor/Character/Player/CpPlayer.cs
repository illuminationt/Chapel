using ImGuiNET;
using Oddworm.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CpPilotComponent))]
[RequireComponent(typeof(CpShootComponent))]
public class CpPlayer : CpCharacterBase, ICpGameplayEffectReciever
{
    CpGameplayEffectHandler _gameplayEffectHandler = null;
    Transform _transform = null;
    CpPilotComponent _pilotComponent = null;
    CpShootComponent _shootComponent = null;
    CpDebugComponent _debugComponent = null;
    CpPlayerForwardCalculator _forwardCalculator = null;

    List<Collider2D> _colliders = new List<Collider2D>();
    public static CpPlayer Get() => CpGameManager.Instance.Player;


    protected override void Awake()
    {
        base.Awake();

        _gameplayEffectHandler = new CpGameplayEffectHandler();

        _transform = GetComponent<Transform>();
        _pilotComponent = GetComponent<CpPilotComponent>();
        _shootComponent = GetComponent<CpShootComponent>();
        _shootComponent.Initialize(this);
        _forwardCalculator = new CpPlayerForwardCalculator(_transform);
        _colliders = GetComponents<Collider2D>().ToList();
    }

    private void Start()
    {

    }

    protected override void Update()
    {
        base.Update();
        _pilotComponent.Execute();
        _forwardCalculator.Update();
        debugDrawDirection();

        updateShoot();


    }


    public void SetCollisionEnabled(bool bEnabled)
    {
        foreach (Collider2D collider in _colliders)
        {
            collider.enabled = bEnabled;
        }
    }

    // ICpActorForwardInterface
    public override float GetForwardDegree()
    {
        return ForwardCalculator.GetForwardDegree();
    }
    // end of ICpActorForwardInterface

    // ICpAttackSendable
    public override ECpAttackSenderGroup GetAttackSenderGroup()
    {
        return ECpAttackSenderGroup.Player;
    }
    public override FCpAttackSendParam CreateAttackSendParam()
    {
        FCpAttackSendParam sendParam;
        sendParam.Attack = 1f;
        return sendParam;
    }
    // end of ICpAttackSendable

    // ICpAttackReceivable
    public override ECpAttackReceiverGroup GetAttackReceiverGroup()
    {
        return ECpAttackReceiverGroup.Player;
    }
    public override void OnReceiveAttack(in FCpAttackSendParam attackSendParam)
    {
        CpDebug.LogError("自機がダメージ受けた");
    }
    // end of ICpAttackReceivable

    //  ICpGameplayEffectReciever
    public CpGameplayEffectHandler GetGameplayEffectHandler() { return _gameplayEffectHandler; }

    // end of  ICpGameplayEffectReciever
    CpPlayerForwardCalculator ForwardCalculator
    {
        get
        {
            if (_forwardCalculator == null)
            {
                _forwardCalculator = new CpPlayerForwardCalculator(transform);
            }
            return _forwardCalculator;
        }
    }

    void updateShoot()
    {
        FCpShootControlParam shootControlParam = new FCpShootControlParam();
        shootControlParam.Player = this;
        shootControlParam.origin = _transform.position;
        shootControlParam.forward = _forwardCalculator.GetForwardVector();
        _shootComponent.execute(shootControlParam);
    }

    void debugDrawDirection()
    {
        Vector2 dir = _forwardCalculator.GetForwardVector();
        Vector2 start = _transform.position;
        Vector2 end = start + dir * 22f;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);


        CpRoomTransitionTrigger roomTransition = collision.GetComponent<CpRoomTransitionTrigger>();
        roomTransition?.OnTrigger();

        ICpGameplayEffectSender sender = collision as ICpGameplayEffectSender;

    }

#if DEBUG
    public void DrawImGui()
    {
        if (ImGui.TreeNode("Shoot Component"))
        {
            _shootComponent.DrawImGui();
            ImGui.TreePop();
        }
    }
#endif
}
