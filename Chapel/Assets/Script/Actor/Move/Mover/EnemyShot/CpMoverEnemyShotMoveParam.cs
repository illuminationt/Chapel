using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

[Inspectable]
[System.Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamEnemyShotMoveParam : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.EnemyShotMoveParam;
}

public class CpMoverEnemyShotMoveParam : CpMoverBase
{
    public static CpMoverEnemyShotMoveParam Create(in FCpMoveParamEnemyShotMoveParam param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverEnemyShotMoveParam>(context);
        mover._param = param;
        return mover;
    }

    public override void GetPosValue(out ECpMoverPositionType outPosType, out Vector2 outValue)
    {
        Assert.IsTrue(false);
        outPosType = ECpMoverPositionType.Velocity;
        outValue = Vector2.zero;
    }

    public override void GetYawValue(out ECpMoverRotationType outYawType, out float outValue)
    {
        outYawType = ECpMoverRotationType.NoYaw;
        outValue = 0f;

    }

    FCpMoveParamEnemyShotMoveParam _param;
}
