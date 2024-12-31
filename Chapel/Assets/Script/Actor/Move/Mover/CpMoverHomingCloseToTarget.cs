using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CpMoverHomingCloseToTarget : CpMoverBase
{
    public static CpMoverHomingCloseToTarget Create(in FCpMoveParamHomingCloseToTarget param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverHomingCloseToTarget>(context);
        mover._param = param;
        return mover;
    }
    protected override void InitializeInternal()
    {
        _newVelocity = GetCurrentOwnerActorPosition();
    }

    protected override void UpdateInternal()
    {
        Vector2 currentVelocity = GetCurrentOwnerVelocity();

        Vector2 targetPosition = CpHomingUtil.CalcHomingTargetLocation(_param.Target);
        Vector2 myPosition = GetCurrentOwnerActorPosition();
        Vector2 toTarget = (targetPosition - myPosition).normalized;

        _newVelocity = currentVelocity + toTarget * _param.Accel * CpTime.DeltaTime;

        if (_newVelocity.magnitude > _param.MaxSpeed)
        {
            _newVelocity = _newVelocity.normalized * _param.MaxSpeed;
        }
    }

    public override Vector2 GetVelocity()
    {
        return _newVelocity;
    }

    Vector2 _newVelocity;
    FCpMoveParamHomingCloseToTarget _param;
}
