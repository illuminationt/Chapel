using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CpMoveParamLinear : CpMoveParamBase
{
    public FCpMoveParamLinear ParamLinear;
}

[Inspectable]
[System.Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamLinear : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.Linear;

    [Inspectable] public float Speed;
    [Inspectable] public float MaxSpeedf;
    [Inspectable] public float DegreeOffset;
    [Inspectable] public float Accel;
    [Inspectable] public CpMoveTrajectoryOnHitWallParam TrajectoryOnHitWallParam;
}

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
        _initialDegree = Context.InitialOwnerDegree + _paramLinear.DegreeOffset;
        _currentDegree = _initialDegree;
    }

    protected override void UpdateInternal()
    {
        _currentSpeed += _paramLinear.Accel * CpTime.DeltaTime;
    }

    public override void GetPosValue(out ECpMoverPositionType outPosType, out Vector2 outValue)
    {
        outPosType = ECpMoverPositionType.Velocity;

        outValue = CalcVelocity();
    }

    public override void GetYawValue(out ECpMoverRotationType outYawType, out float outValue)
    {
        outYawType = ECpMoverRotationType.NoYaw;
        outValue = 0f;
    }

    public override void OnCollisionEnterHit2D(Collision2D collision)
    {
        Transform owner = Context.OwnerActor.transform;
        Vector2 currentVel = CalcVelocity();
        Vector2 newVel = Vector2.zero;
        bool bValidResult = _paramLinear.TrajectoryOnHitWallParam.GetNewVelocityOnHitWall(owner, currentVel, collision, ref newVel);
        if (bValidResult)
        {
            _currentSpeed = newVel.magnitude;
            _currentDegree = SltMath.ToDegree(newVel.normalized);
        }
    }

    Vector2 CalcVelocity()
    {
        Vector2 dir = SltMath.ToVector(_currentDegree);
        return dir * _currentSpeed;
    }

    float _currentSpeed = 0f;
    float _initialDegree = 0f;
    float _currentDegree = 0f;
    FCpMoveParamLinear _paramLinear;
}
