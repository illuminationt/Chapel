using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpMoverLinear : CpMoverBase
{
    public static CpMoverLinear Create(in FCpMoveParamLinear param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverLinear>(context);
        mover._paramLinear = param;
        return mover;
    }

    protected override void InitializeInternal()
    {
        _currentSpeed = _paramLinear.Speed;
        _initialDegree = Context.OwnerDegree;
    }

    protected override void UpdateInternal()
    {
        _currentSpeed += _paramLinear.Accel * CpTime.DeltaTime;
    }

    public override Vector2 GetVelocity()
    {
        Vector2 dir = SltMath.ToVector(_initialDegree + _paramLinear.DegreeOffset);
        return dir * _currentSpeed;
    }

    float _currentSpeed = 0f;
    float _initialDegree = 0f;
    FCpMoveParamLinear _paramLinear;
}
