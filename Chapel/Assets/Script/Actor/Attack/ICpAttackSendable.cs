using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FCpAttackSendParam
{
    public float Attack;
}

public interface ICpAttackSendable
{
    public ECpAttackSenderGroup GetAttackSenderGroup();
    public virtual void OnSendAttack() { }
    public abstract FCpAttackSendParam CreateAttackSendParam();
}
