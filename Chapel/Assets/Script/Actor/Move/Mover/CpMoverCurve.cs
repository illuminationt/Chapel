using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpMoverCurve : CpMoverBase
{
    public static CpMoverCurve Create(in FCpMoveParamCurve param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverCurve>(context);
        mover._paramCurve = param;
        return mover;
    }

    protected override void InitializeInternal()
    {
        _currentSpeed = _paramCurve.Speed;
        _currentDegree = _paramCurve.Degree;
    }

    protected override void UpdateInternal()
    {
        _currentSpeed += _paramCurve.Accel * CpTime.DeltaTime;
    }

    public override Vector2 GetVelocity()
    {
        Vector2 dir = SltMath.ToVector(_currentDegree);
        return dir * _currentSpeed;
    }

    float _currentSpeed;
    float _currentDegree;
    FCpMoveParamCurve _paramCurve;
}
