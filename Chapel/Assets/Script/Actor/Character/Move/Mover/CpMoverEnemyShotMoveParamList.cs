using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CpMoverEnemyShotMoveParamList : CpMoverBase
{
    public static CpMoverEnemyShotMoveParamList Create(in FCpMoveParamEnemyShotMoveParamList param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverEnemyShotMoveParamList>(context);
        mover._param = param;
        return mover;
    }

    public override Vector2 GetVelocity()
    {
        Assert.IsTrue(false);
        return Vector2.zero;
    }

    FCpMoveParamEnemyShotMoveParamList _param;
}
