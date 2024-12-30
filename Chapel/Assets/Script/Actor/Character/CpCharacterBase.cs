using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CpCharacterBase : CpActorBase,
    ICpAttackSendable,
    ICpAttackReceivable
{
    float _hp = 1f;
    protected virtual void OnDead()
    {
        Destroy(gameObject);
    }

    // ICpAttackSendable
    public FCpAttackSendParam CreateAttackSendParam() { throw new NotImplementedException(); }
    // end of ICpAttackSendable

    // ICpAttackReceivable
    public void OnReceiveAttack(in FCpAttackSendParam attackSendParam)
    {
        _hp -= attackSendParam.Attack;
        if (_hp <= 0f)
        {
            OnDead();
        }
    }
    // end of ICpAttackReceivable

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ICpAttackReceivable otherAttackReceivable = collision.gameObject.GetComponent<ICpAttackReceivable>();
        if (otherAttackReceivable != null)
        {
            ICpAttackSendable attackSendabgle = this;
            attackSendabgle.SendAttack(otherAttackReceivable);
        }
    }
}
