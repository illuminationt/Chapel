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

    public FCpMoverId RequestStart(CpMoveParamBase moveParam)
    {
        CpMoverBase newMover = CreateMover(moveParam);
        OnMoverCreated(newMover);
        return newMover.GetId();
    }

    public FCpMoverId RequestStart(in FCpMoveParamLinear moveParamLinear)
    {
        CpMoverBase newMover = CpMoverLinear.Create(moveParamLinear, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();
    }

    public FCpMoverId RequestStart(in FCpMoveParamCurve moveParamCurve)
    {
        CpMoverBase newMover = CpMoverCurve.Create(moveParamCurve, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();

    }
    public FCpMoverId RequestStart(in FCpMoveParamTween moveParamTween)
    {
        CpMoverBase newMover = CpMoverTween.Create(moveParamTween, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();

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
        CpMoverBase newMover = CpMoverEnemyShotDefault.Create(moveParamEnemyShotDefault, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();

    }
    public FCpMoverId RequestStart(in FCpMoveParamEnemyShotMoveParam param)
    {
        CpMoverBase newMover = CpMoverEnemyShotMoveParam.Create(param, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();

    }
    public FCpMoverId RequestStart(in FCpMoveParamEnemyShotMoveParamList param)
    {
        CpMoverBase newMover = CpMoverEnemyShotMoveParamList.Create(param, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();
    }

    public FCpMoverId RequestStart(in FCpMoveParamHomingCloseToTarget param)
    {
        CpMoverBase newMover = CpMoverHomingCloseToTarget.Create(param, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();
    }
    public FCpMoverId RequestStart(in FCpMoveParamHomingForceClose param)
    {
        CpMoverBase newMover = CpMoverHomingForceClose.Create(param, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();
    }
    public FCpMoverId RequestStart(in FCpMoveParamHomingOnlyRotate param)
    {
        CpMoverBase newMover = CpMoverHomingOnlyRotate.Create(param, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();
    }

    public FCpMoverId RequestStart(in FCpMoveParamPhysical param)
    {
        CpMoverBase newMover = CpMoverPhysical.Create(param, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();
    }

    public FCpMoverId RequestStart(in FCpMoveParamRotate param)
    {
        CpMoverBase newMover = CpMoverRotateOnce.Create(param, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();
    }
    public FCpMoverId RequestStart(in FCpMoveParamRotateInfFixedDirection param)
    {
        CpMoverBase newMover = CpMoverRotateInfinitelyFixedDirection.Create(param, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();
    }
    public FCpMoverId RequestStart(in FCpMoveParamRotateInfToTarget param)
    {
        CpMoverBase newMover = CpMoverRotateInfinitelyToTarget.Create(param, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();
    }

    public FCpMoverId RequestStart(in FCpMoveParamTrampoline param)
    {
        CpMoverBase newMover = CpMoverEnemyTrampoline.Create(param, CreateContext());
        OnMoverCreated(newMover);
        return newMover.GetId();
    }

    void OnMoverCreated(CpMoverBase newMover)
    {
        if (ExistsActiveMover(newMover.GetType()))
        {
            Assert.IsTrue(false, $"Mover.GetType()=={newMover.GetType()}Ç™ÇΩÇ≠Ç≥ÇÒ");
            return;
        }

        if (_currentMovers.Count > 3)
        {
            Assert.IsTrue(false, "MoverÇÕç≈ëÂÇRÇ¬Ç‹Ç≈");
        }
        _currentMovers.Add(newMover);
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
            case ECpMoveParamType.HomingForceClose: return RequestStart((FCpMoveParamHomingForceClose)imove);
            case ECpMoveParamType.Physical: return RequestStart((FCpMoveParamPhysical)imove);

            case ECpMoveParamType.Rotate: return RequestStart((FCpMoveParamRotate)imove);
            case ECpMoveParamType.RotateInfFixedDirection: return RequestStart((FCpMoveParamRotateInfFixedDirection)imove);
            case ECpMoveParamType.RotateInfToTarget: return RequestStart((FCpMoveParamRotateInfToTarget)imove);

            case ECpMoveParamType.Trampoline: return RequestStart((FCpMoveParamTrampoline)imove);

            default:
                Assert.IsTrue(false, $"{moveParamType}Ç™ñ¢é¿ëï");
                return FCpMoverId.INVALID_ID;
        }
    }
}

