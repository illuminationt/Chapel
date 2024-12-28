using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public interface ICpMover
{
    public abstract CpMoveComponent GetMoveComponent();

    public void StartMove(CpMoveParamBase moveParam)
    {
        GetMoveComponent().RequestStart(moveParam);
    }

    public void StartMove(in FCpMoveParam moveParam)
    {
        switch (moveParam.MoveParamType)
        {
            case ECpMoveParamType.None:
                Assert.IsTrue(false);
                break;

            case ECpMoveParamType.Linear:
                StartMove(moveParam.MoveParamLinear);
                break;

            case ECpMoveParamType.Curve:
                StartMove(moveParam.MoveParamCurve);
                break;
        }
    }

    public void StartMove(in FCpMoveParamLinear moveParamLinear)
    {
        GetMoveComponent().RequestStart(moveParamLinear);
    }

    public void StartMove(in FCpMoveParamCurve moveParamCurve)
    {
        GetMoveComponent().RequestStart(moveParamCurve);
    }
}