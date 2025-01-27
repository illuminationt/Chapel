using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CpPlayerShot : CpShotBase
{
    protected override void Release()
    {
        CpObjectPool pool = CpObjectPool.Get();
        pool.Release(this);
    }

    public void OnCreated(CpPlayerWeaponShotGeneralParam generalParam, in FCpShootControlParam controlParam)
    {
        _lifeTime = generalParam.LifeTime;
        _forwardDegreeOnCreated = SltMath.ToDegree(controlParam.forward);

        SetRotationToVelocity(SltMath.ToVector(_forwardDegreeOnCreated));
    }

    // CpActorBase interface
    public override ECpMoverUpdateType GetMoverUpdateType() { return ECpMoverUpdateType.FixedUpdateFunction; }

    // end of CpActorBase interface

    // start ICpForwardInterface
    public override float GetForwardDegree()
    {
        return _forwardDegreeOnCreated;
    }
    // end of ICpForwardInterface

    // ICpAttackSendable
    public override ECpAttackSenderGroup GetAttackSenderGroup()
    {
        return ECpAttackSenderGroup.PlayerShot;
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
        return ECpAttackReceiverGroup.PlayerShot;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        int layer = collision.gameObject.layer;
        if (layer == CpLayer.EnemyShield)
        {
            Release();
        }
    }

    float _forwardDegreeOnCreated = 0f;
}
