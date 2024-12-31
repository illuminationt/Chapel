using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class CpCharacterBase : CpActorBase,
    ICpAttackSendable,
    ICpAttackReceivable
{
    [SerializeField]
    float _hp = 1f;
    protected virtual void OnDead()
    {
        Destroy(gameObject);
    }

    // ICpAttackSendable
    public virtual ECpAttackSenderGroup GetAttackSenderGroup()
    {
        Assert.IsTrue(false);
        return ECpAttackSenderGroup.None;
    }
    public virtual FCpAttackSendParam CreateAttackSendParam()
    {
        Assert.IsTrue(false, GetType().Name + "‚ÌCreateAttackSendParam ‚ªŽÀ‘•‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
        throw new NotImplementedException();
    }
    // end of ICpAttackSendable

    // ICpAttackReceivable
    public virtual ECpAttackReceiverGroup GetAttackReceiverGroup()
    {
        Assert.IsTrue(false);
        return ECpAttackReceiverGroup.None;
    }
    public virtual void OnReceiveAttack(in FCpAttackSendParam attackSendParam)
    {
        _hp -= attackSendParam.Attack;
        if (_hp <= 0f)
        {
            OnDead();
        }
    }
    // end of ICpAttackReceivable

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICpAttackReceivable otherAttackReceivable = collision.gameObject.GetComponent<ICpAttackReceivable>();

        CpAttackUtil.OnTriggerEnter2D(this, otherAttackReceivable, collision);
    }
}
