using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CpMoveComponent))]
public class CpShotBase : CpActorBase,
    ICpMover,
    ICpAttackSendable,
    ICpAttackReceivable
{
    public CpMoveComponent _moveComponent = null;

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
    public FCpAttackSendParam CreateAttackSendParam()
    {
        FCpAttackSendParam sendParam;
        sendParam.Attack = 1f;
        return sendParam;
    }
    // end of ICpAttackSendable

    // ICpAttackReceivable
    public void OnReceiveAttack(in FCpAttackSendParam attackSendParam)
    {
        Destroy(gameObject);
    }
    // end of ICpAttackReceivable
}
