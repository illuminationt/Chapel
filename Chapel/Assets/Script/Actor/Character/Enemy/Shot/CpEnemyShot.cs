using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CpEnemyShotInitializeParam
{
    public Vector2 SpawnPosition = Vector2.zero;
    public FCpMoveParamEnemyShot enemyShotMoveParam;
    public ECpEnemyShotRotationType RotationType = ECpEnemyShotRotationType.Noop;
    public float Scale = 1f;
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

        _transform.SetLocalScale(_initParam.Scale);
        StartMove(initializeParam.enemyShotMoveParam);
    }


    public void Erase()
    {
        Release();
    }

    void StartMove(in FCpMoveParamEnemyShot moveParam)
    {
        CpMoveComponent moveComp = GetComponent<CpMoveComponent>();
        moveComp.RequestStart(moveParam);
    }

    CpEnemyShotInitializeParam _initParam = null;
}
