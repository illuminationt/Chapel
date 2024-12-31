using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CpEnemyShotInitializeParam
{
    public FCpMoveParamEnemyShot enemyShotMoveParam;
}


public class CpEnemyShot : CpShotBase
{

    // ICpAttackSendable
    public override ECpAttackSenderGroup GetAttackSenderGroup()
    {
        return ECpAttackSenderGroup.EnemyShot;
    }
    public override FCpAttackSendParam CreateAttackSendParam()
    {
        FCpAttackSendParam sendParam;
        sendParam.Attack = 1f;
        return sendParam;
    }
    // end of ICpAttackSendable

    public override ECpAttackReceiverGroup GetAttackReceiverGroup()
    {
        return ECpAttackReceiverGroup.EnemyShot;
    }

    public void Initialize(CpEnemyShotInitializeParam initializeParam)
    {
        StartMove(initializeParam.enemyShotMoveParam);
    }

    void StartMove(in FCpMoveParamEnemyShot moveParam)
    {
        CpMoveComponent moveComp = GetComponent<CpMoveComponent>();
        moveComp.RequestStart(moveParam);
    }
}
