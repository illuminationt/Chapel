using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public partial class CpMoverManager
{
    CpMoverBase CreateMover(CpMoveParamBase moveParam)
    {
        FCpMoverContext context = CreateContext();

        CpMoveParamLinear paramLinear = moveParam as CpMoveParamLinear;
        if (paramLinear != null)
        {
            return CpMoverLinear.Create(paramLinear.ParamLinear, context);
        }

        CpMoveParamCurve paramCurve = moveParam as CpMoveParamCurve;
        if (paramCurve != null)
        {
            return CpMoverCurve.Create(paramCurve.ParamCurve, context);
        }

        Assert.IsTrue(false);
        return null;
    }

    public void RequestStart(in FCpMoveParamLinear moveParamLinear)
    {
        _currentMover = CpMoverLinear.Create(moveParamLinear, CreateContext());
    }

    public void RequestStart(in FCpMoveParamCurve moveParamCurve)
    {
        _currentMover = CpMoverCurve.Create(moveParamCurve, CreateContext());
    }
    public void RequestStart(in FCpMoveParamTween moveParamTween)
    {
        _currentMover = CpMoverTween.Create(moveParamTween, CreateContext());
    }
    public void RequestStart(in FCpMoveParamEnemyShot param)
    {
        switch (param.MoveType)
        {
            case ECpEnemyShotMoveType.Default:
                RequestStart(param.ParamDefault);
                break;

            case ECpEnemyShotMoveType.MoveParam:
                RequestStart(param.ParamMoveParam);
                break;

            case ECpEnemyShotMoveType.MoveParamList:
                RequestStart(param.ParamMoveParamList);
                break;

            default:
                Assert.IsTrue(false);
                break;
        }
    }
    public void RequestStart(in FCpMoveParamEnemyShotDefault moveParamEnemyShotDefault)
    {
        _currentMover = CpMoverEnemyShotDefault.Create(moveParamEnemyShotDefault, CreateContext());
    }
    public void RequestStart(in FCpMoveParamEnemyShotMoveParam param)
    {
        _currentMover = CpMoverEnemyShotMoveParam.Create(param, CreateContext());
    }
    public void RequestStart(in FCpMoveParamEnemyShotMoveParamList param)
    {
        _currentMover = CpMoverEnemyShotMoveParamList.Create(param, CreateContext());
    }
}

