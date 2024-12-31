using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CpTaskComponent))]
public class CpEnemyBase : CpCharacterBase
{
    public void Start()
    {
        CpTaskComponent taskComp = GetComponent<CpTaskComponent>();
        taskComp.StartStateMachine();
    }

    public override float GetForwardDegree()
    {
        return 0f;
    }

    // ICpAttackSendable
    public override ECpAttackSenderGroup GetAttackSenderGroup()
    {
        return ECpAttackSenderGroup.Enemy;
    }
    public override FCpAttackSendParam CreateAttackSendParam()
    {
        FCpAttackSendParam sendParam;
        sendParam.Attack = 1f;
        return sendParam;
    }
    // end of ICpAttackSendable

    // IC
    public override ECpAttackReceiverGroup GetAttackReceiverGroup()
    {
        return ECpAttackReceiverGroup.Enemy;
    }
    public override void OnReceiveAttack(in FCpAttackSendParam attackSendParam)
    {
        base.OnReceiveAttack(attackSendParam);
    }
}
