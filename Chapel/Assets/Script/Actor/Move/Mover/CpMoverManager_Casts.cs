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

    public FCpMoverId RequestStart(in FCpMoveParamLinear moveParamLinear)
    {
        _currentMover = CpMoverLinear.Create(moveParamLinear, CreateContext());
        return _currentMover.GetId();
    }

    public FCpMoverId RequestStart(in FCpMoveParamCurve moveParamCurve)
    {
        _currentMover = CpMoverCurve.Create(moveParamCurve, CreateContext());
        return _currentMover.GetId();

    }
    public FCpMoverId RequestStart(in FCpMoveParamTween moveParamTween)
    {
        _currentMover = CpMoverTween.Create(moveParamTween, CreateContext());
        return _currentMover.GetId();

    }
    public FCpMoverId RequestStart(in FCpMoveParamEnemyShot param)
    {
        switch (param.MoveType)
        {
            case ECpEnemyShotMoveType.Default:
                return RequestStart(param.ParamDefault);

            case ECpEnemyShotMoveType.MoveParam:
                return RequestStart(param.ParamMoveParam);

            case ECpEnemyShotMoveType.MoveParamList:
                return RequestStart(param.ParamMoveParamList);

            default:
                Assert.IsTrue(false);
                return FCpMoverId.INVALID_ID;
        }
    }
    public FCpMoverId RequestStart(in FCpMoveParamEnemyShotDefault moveParamEnemyShotDefault)
    {
        _currentMover = CpMoverEnemyShotDefault.Create(moveParamEnemyShotDefault, CreateContext());
        return _currentMover.GetId();

    }
    public FCpMoverId RequestStart(in FCpMoveParamEnemyShotMoveParam param)
    {
        _currentMover = CpMoverEnemyShotMoveParam.Create(param, CreateContext());
        return _currentMover.GetId();

    }
    public FCpMoverId RequestStart(in FCpMoveParamEnemyShotMoveParamList param)
    {
        _currentMover = CpMoverEnemyShotMoveParamList.Create(param, CreateContext());
        return _currentMover.GetId();
    }

    public FCpMoverId RequestStart(in FCpMoveParamHomingCloseToTarget param)
    {
        _currentMover = CpMoverHomingCloseToTarget.Create(param, CreateContext());
        return _currentMover.GetId();
    }
    public FCpMoverId RequestStart(in FCpMoveParamHomingOnlyRotate param)
    {
        _currentMover = CpMoverHomingOnlyRotate.Create(param, CreateContext());
        return _currentMover.GetId();
    }
    public FCpMoverId RequestStart(ICpMoveParam imove)
    {
        ECpMoveParamType moveParamType = imove.GetMoveParamType();
        switch (moveParamType)
        {
            case ECpMoveParamType.Linear: return RequestStart((FCpMoveParamLinear)imove);
            case ECpMoveParamType.Curve: return RequestStart((FCpMoveParamCurve)imove);
            case ECpMoveParamType.Tween: return RequestStart((FCpMoveParamTween)imove);
            case ECpMoveParamType.EnemyShotDefault: return RequestStart((FCpMoveParamEnemyShotDefault)imove);
            case ECpMoveParamType.EnemyShotMoveParam: return RequestStart((FCpMoveParamEnemyShotMoveParam)imove);
            case ECpMoveParamType.EnemyShotMoveParamList: return RequestStart((FCpMoveParamEnemyShotMoveParamList)imove);
            case ECpMoveParamType.HomingOnlyRotate: return RequestStart((FCpMoveParamHomingOnlyRotate)imove);
            case ECpMoveParamType.HomingCloseToTarget: return RequestStart((FCpMoveParamHomingCloseToTarget)imove);
            default:
                Assert.IsTrue(false, "ああああ！！！！！！！");
                return FCpMoverId.INVALID_ID;
        }
    }



}

