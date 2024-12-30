using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CpMoverEnemyShotMoveParam : CpMoverBase
{
    public static CpMoverEnemyShotMoveParam Create(in FCpMoveParamEnemyShotMoveParam param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverEnemyShotMoveParam>(context);
        mover._param = param;
        return mover;
    }

    public override Vector2 GetVelocity()
    {
        Assert.IsTrue(false);
        return Vector2.zero;
    }

    FCpMoveParamEnemyShotMoveParam _param;
}
