using Oddworm.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CpPilotComponent))]
[RequireComponent(typeof(CpShootComponent))]
public class CpPlayer : CpActorBase
{
    Transform _transform = null;
    CpPilotComponent _pilotComponent = null;
    CpShootComponent _shootComponent = null;
    CpDebugComponent _debugComponent = null;
    CpPlayerForwardCalculator _forwardCalculator = null;

    public CpHellParamScriptableObject HellParamScriptableObject = null;
    private void Start()
    {
        _transform = GetComponent<Transform>();
        _pilotComponent = GetComponent<CpPilotComponent>();
        _shootComponent = GetComponent<CpShootComponent>();
        _forwardCalculator = new CpPlayerForwardCalculator(_transform);
    }

    private void Update()
    {
        _pilotComponent.execute();
        _forwardCalculator.execute();
        debugDrawDirection();

        updateShoot();

        if (Input.GetKeyDown(KeyCode.M))
        {
            CpTaskComponent taskComp = GetComponent<CpTaskComponent>();
            taskComp.StartStateMachine();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            CpHellComponent hellComp = GetComponent<CpHellComponent>();
            hellComp.RequestStart(HellParamScriptableObject.MultiHellParam);
        }
    }

    // ICpActorForwardInterface
    public override float GetForwardDegree()
    {
        return ForwardCalculator.GetForwardDegree();
    }
    // end of ICpActorForwardInterface

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

    // ICpAttackSendable
    public FCpAttackSendParam CreateAttackSendParam()
    {
        FCpAttackSendParam sendParam;
        sendParam.Attack = 1f;
        return sendParam;
    }
    // end of ICpAttackSendable

    // ICpAttackReceivable
    public void OnReceiveAttack(in FCpAttackSendParam attackSendParam)
    {
        CpDebug.LogError("���@���_���[�W�󂯂�");
    }
    // end of ICpAttackReceivable
}
