using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

[RequireComponent(typeof(CpMoveComponent))]
public abstract class CpShotBase : CpActorBase,
    ICpMover,
    ICpAttackSendable,
    ICpAttackReceivable
{
    float _elapsedTime = 0f;
    protected float _lifeTime = 0f;
    protected override void Update()
    {
        base.Update();

        if (_lifeTime > 0f)
        {

            _elapsedTime += CpTime.DeltaTime;
            if (_elapsedTime > _lifeTime)
            {
                Release();
            }
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
        Destroy(gameObject);
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
        Destroy(gameObject);
    }
    // end of ICpAttackReceivable

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICpAttackReceivable otherAttackReceivable = collision.gameObject.GetComponent<ICpAttackReceivable>();
        CpAttackUtil.OnTriggerEnter2D(this, otherAttackReceivable, collision);
    }


    public CpMoveComponent _moveComponent = null;
}
