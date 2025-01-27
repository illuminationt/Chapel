using Oddworm.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ImGuiNET;

[RequireComponent(typeof(CpPilotComponent))]
[RequireComponent(typeof(CpShootComponent))]
public class CpPlayer : CpCharacterBase
    , ICpGameplayEffectReciever
    , ICpAbsorbable
{
    CpGameplayEffectHandler _gameplayEffectHandler = null;
    CpPilotComponent _pilotComponent = null;
    CpShootComponent _shootComponent = null;
    CpDebugComponent _debugComponent = null;
    CpPlayerLockonController _lockonController = null;
    CpPlayerForwardCalculator _forwardCalculator = null;
    CpPredictiveLine _predictiveLine = null;
    [SerializeField] CpPredictiveLine _predictiveLinePrefab = null;
    List<Collider2D> _colliders = new List<Collider2D>();
    public static CpPlayer Get() => CpGameManager.Instance.Player;


    protected override void Awake()
    {
        base.Awake();

        _gameplayEffectHandler = new CpGameplayEffectHandler();

        _pilotComponent = GetComponent<CpPilotComponent>();
        _shootComponent = GetComponent<CpShootComponent>();
        _shootComponent.Initialize(this);
        _lockonController = CpPlayerLockonController.Create(_transform, this);
        _forwardCalculator = CpPlayerForwardCalculator.Create(_transform, _lockonController);
        _colliders = GetComponents<Collider2D>().ToList();

        _predictiveLine = CpPredictiveLine.Create(_predictiveLinePrefab, this);
    }

    protected override void Update()
    {
        base.Update();
        _pilotComponent.Execute();
        _forwardCalculator.Update();

        float forwardDeg = _forwardCalculator.GetForwardDegree();
        transform.SetRotation(forwardDeg);

        updateShoot();

        _predictiveLine.execute(forwardDeg);


        // ロックオンいったんここに書く
        updateLockon();
    }

    // CpActorBase interface
    public override ECpMoverUpdateType GetMoverUpdateType() { return ECpMoverUpdateType.UpdateFunction; }

    // end of CpActorBase interface

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
        CpDebug.LogWarning("自機がダメージ受けた");
    }
    // end of ICpAttackReceivable

    //  ICpGameplayEffectReciever
    public CpGameplayEffectHandler GetGameplayEffectHandler() { return _gameplayEffectHandler; }

    // end of  ICpGameplayEffectReciever

    // ICpAbsorbable
    public Transform GetAbsorbRootTransform() { return _transform; }
    // end of ICpAbsorbable

    public CpShootComponent GetShootComponent() => _shootComponent;
    CpPlayerForwardCalculator ForwardCalculator
    {
        get
        {
            if (_forwardCalculator == null)
            {
                _forwardCalculator = CpPlayerForwardCalculator.Create(transform, _lockonController);
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

    void updateLockon()
    {
        // _lockonController.Update();

        CpInputManager input = CpInputManager.Get();
        if (input.WasPressed(ECpButton.Lockon))
        {
            _lockonController.Toggle();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        CpRoomTransitionTrigger roomTransition = collision.GetComponent<CpRoomTransitionTrigger>();
        roomTransition?.OnTrigger();

        ICpGameplayEffectSender sender = collision as ICpGameplayEffectSender;
    }

#if CP_DEBUG
    public void DrawImGui()
    {
        SltImGui.TextVector2("Position", transform.position, 1, 4);

        if (ImGui.TreeNode("Shoot Component"))
        {
            _shootComponent.DrawImGui();
            ImGui.TreePop();
        }
        if (ImGui.TreeNode("Forward Calculator"))
        {
            _forwardCalculator.DrawImGui();
            ImGui.TreePop();
        }
    }
#endif
}
