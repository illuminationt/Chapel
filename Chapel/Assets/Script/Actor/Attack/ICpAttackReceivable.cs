using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICpAttackReceivable
{
    public void OnReceiveAttack(in FCpAttackSendParam attackSendParam);
}
