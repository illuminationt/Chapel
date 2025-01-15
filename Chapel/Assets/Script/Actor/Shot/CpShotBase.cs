using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

[RequireComponent(typeof(CpMoveComponent))]
public abstract class CpShotBase : CpActorBase,
    ICpMover,
    ICpAttackSendable,
    ICpAttackReceivable
{
    protected virtual void FixedUpdate()
    {
        if (_lifeTime > 0f)
        {
            _elapsedTime += CpTime.DeltaTime;
            if (_elapsedTime > _lifeTime)
            {
                Release();
            }
        }

        if (!CpUtil.IsInScreen(_transform.position, 0f))
        {
            Release();
        }
    }

    // start ICpMover interface
    public CpMoveComponent GetMoveComponent() { return _moveComponent; }

    // end of ICpMover

    // start ICpForwardInterface
    public override float GetForwardDegree()
    {
        return 0f;
    }
    // end of ICpForwardInterface

    // ICpAttackSendable
    public virtual ECpAttackSenderGroup GetAttackSenderGroup()
    {
        Assert.IsTrue(false);
        return ECpAttackSenderGroup.None;
    }
    public virtual void OnSendAttack()
    {
        Release();
    }

    public virtual FCpAttackSendParam CreateAttackSendParam()
    {
        FCpAttackSendParam sendParam;
        sendParam.Attack = 1f;
        return sendParam;
    }
    // end of ICpAttackSendable

    // ICpAttackReceivable
    public virtual ECpAttackReceiverGroup GetAttackReceiverGroup()
    {
        throw new System.NotImplementedException();
    }
    public void OnReceiveAttack(in FCpAttackSendParam attackSendParam)
    {
        Release();
    }
    // end of ICpAttackReceivable

    // ICpPoolable
    public override void ResetOnRelease()
    {
        base.ResetOnRelease();
        _elapsedTime = 0f;
    }
    // end of 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICpAttackReceivable otherAttackReceivable = collision.gameObject.GetComponent<ICpAttackReceivable>();
        CpAttackUtil.OnTriggerEnter2D(this, otherAttackReceivable, collision);

        int layer = collision.gameObject.layer;
        if (layer == CpLayer.Wall)
        {
            Release();
        }
    }

    protected void SetRotationToVelocity(in Vector2 dir)
    {
        float yaw = SltMath.ToDegree(dir);
        transform.SetRotation(yaw);
    }

    float _elapsedTime = 0f;
    protected float _lifeTime = 0f;
    public CpMoveComponent _moveComponent = null;
}
