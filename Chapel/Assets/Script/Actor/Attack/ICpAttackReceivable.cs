using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICpAttackReceivable
{
    public ECpAttackReceiverGroup GetAttackReceiverGroup();
    public void OnReceiveAttack(in FCpAttackSendParam attackSendParam);
}
