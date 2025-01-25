using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CpMoveParamCurve : CpMoveParamBase
{
    public FCpMoveParamCurve ParamCurve;
}

[Inspectable]
[System.Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamCurve : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.Curve;

    public float Speed;
    public float MaxSpeed;
    public float Accel;
    public float Degree;
    public float AngularSpeed;
    public float MaxCurveDegree;
}

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

    public override void GetPosValue(out ECpMoverPositionType outPosType, out Vector2 outValue)
    {
        outPosType = ECpMoverPositionType.Velocity;

        Vector2 dir = SltMath.ToVector(_currentDegree);
        outValue = dir * _currentSpeed;
    }
    public override void GetYawValue(out ECpMoverRotationType outYawType, out float outValue)
    {
        outYawType = ECpMoverRotationType.NoYaw;
        outValue = 0f;
    }

    float _currentSpeed;
    float _currentDegree;
    FCpMoveParamCurve _paramCurve;
}
