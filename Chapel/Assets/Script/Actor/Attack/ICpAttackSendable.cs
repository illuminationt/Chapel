using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FCpAttackSendParam
{
    public float Attack;
}

public interface ICpAttackSendable
{
    public abstract FCpAttackSendParam CreateAttackSendParam();
    public void SendAttack(ICpAttackReceivable attackReceivable)
    {
        FCpAttackSendParam attackSendParam = CreateAttackSendParam();
        attackReceivable.OnReceiveAttack(attackSendParam);
    }
}
