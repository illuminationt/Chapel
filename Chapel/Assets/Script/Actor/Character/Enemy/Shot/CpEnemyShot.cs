using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CpEnemyShotInitializeParam
{
    public FCpMoveParamEnemyShot enemyShotMoveParam;
    public ECpEnemyShotRotationType RotationType = ECpEnemyShotRotationType.Noop;
}


public class CpEnemyShot : CpShotBase
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_initParam.RotationType == ECpEnemyShotRotationType.VelocityDirection)
        {
            Vector2 dir = _moveComponent.GetVelocity();
            SetRotationToVelocity(dir);
        }
    }

    protected override void Release()
    {
        CpObjectPool pool = CpObjectPool.Get();
        pool.Release(this);
    }
    public void DebugRelease() { Release(); }

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
        _initParam = initializeParam;
        StartMove(initializeParam.enemyShotMoveParam);
    }

    void StartMove(in FCpMoveParamEnemyShot moveParam)
    {
        CpMoveComponent moveComp = GetComponent<CpMoveComponent>();
        moveComp.RequestStart(moveParam);
    }

    CpEnemyShotInitializeParam _initParam = null;
}
