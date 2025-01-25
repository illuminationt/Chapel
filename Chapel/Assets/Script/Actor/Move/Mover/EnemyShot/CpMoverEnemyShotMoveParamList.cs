using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

[Inspectable]
[System.Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamEnemyShotMoveParamList : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.EnemyShotMoveParamList;

    public FCpMoveParamEnemyShotMoveParam param2;
    public FCpMoveParamEnemyShotDefault basdu;
}
public class CpMoverEnemyShotMoveParamList : CpMoverBase
{
    public static CpMoverEnemyShotMoveParamList Create(in FCpMoveParamEnemyShotMoveParamList param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverEnemyShotMoveParamList>(context);
        mover._param = param;
        return mover;
    }

    public override void GetPosValue(out ECpMoverPositionType outPosType, out Vector2 outValue)
    {
        Assert.IsTrue(false);
        outPosType = ECpMoverPositionType.None;
        outValue = Vector2.zero;
    }
    public override void GetYawValue(out ECpMoverRotationType outYawType, out float outValue)
    {
        outYawType = ECpMoverRotationType.NoYaw;
        outValue = 0f;
    }

    FCpMoveParamEnemyShotMoveParamList _param;
}
