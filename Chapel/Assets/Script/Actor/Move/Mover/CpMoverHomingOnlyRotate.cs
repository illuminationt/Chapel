using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CpMoverHomingOnlyRotate : CpMoverBase
{
    public static CpMoverHomingOnlyRotate Create(in FCpMoveParamHomingOnlyRotate param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverHomingOnlyRotate>(context);
        mover._param = param;
        return mover;
    }

    protected override void InitializeInternal()
    {
        _currentSpeed = _param.Speed;
        _currentDegree = Context.InitialOwnerDegree;
    }

    protected override void UpdateInternal()
    {
        _currentSpeed += _param.Accel * CpTime.DeltaTime;

        Vector2 targetPosition = CalcHomingTargetLocation();
        Vector2 myPosition = Context.OwnerActor.transform.position;
    }

    public override Vector2 GetVelocity()
    {
        Assert.IsTrue(false, "–¢ŽÀ‘•.“ï‚µ‚¢ŽÀ‘•‚É‚È‚é‚©‚çŒã‰ñ‚µ");
        return Vector2.zero;
    }

    Vector2 CalcHomingTargetLocation()
    {
        switch (_param.Target)
        {
            case ECpHomingTarget.Player:
                return CpUtil.GetPlayerWorldPosition();
            default:
                Assert.IsTrue(false);
                return Vector2.zero;
        }
    }

    float _currentSpeed;
    float _currentDegree;

    FCpMoveParamHomingOnlyRotate _param;
}
