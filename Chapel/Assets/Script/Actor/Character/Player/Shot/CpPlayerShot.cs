using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CpPlayerShot : CpShotBase
{
    public void OnCreated(in FCpShootControlParam controlParam)
    {
        _forwardDegreeOnCreated = SltMath.ToDegree(controlParam.forward);
    }

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

    float _forwardDegreeOnCreated = 0f;
}
