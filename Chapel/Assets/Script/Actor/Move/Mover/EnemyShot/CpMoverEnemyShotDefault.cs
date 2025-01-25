using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[Inspectable]
[System.Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamEnemyShotDefault : ICpMoveParam
{
    public FCpMoveParamEnemyShotDefault(
        float speed, in SltFloatInterval speedInterval,
        float accel,
        float angleSpeed, float angleAccel,
        in Vector2 direction
        )
    {
        Speed = speed;
        SpeedInterval = speedInterval;
        Accel = accel;
        AngleSpeed = angleSpeed;
        AngleAccel = angleAccel;
        Direction = direction;
    }
    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.EnemyShotDefault;

    public float Speed;
    public SltFloatInterval SpeedInterval;
    public float Accel;
    public float AngleSpeed;
    public float AngleAccel;
    public Vector2 Direction;
}

public class CpMoverEnemyShotDefault : CpMoverBase
{
    public static CpMoverEnemyShotDefault Create(in FCpMoveParamEnemyShotDefault param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverEnemyShotDefault>(context);
        mover._param = param;
        return mover;
    }

    protected override void InitializeInternal()
    {
        _currentDegree = SltMath.ToDegree(_param.Direction);
        _currentSpeed = _param.Speed;
        _currentAngleSpeed = _param.AngleAccel;
    }

    protected override void UpdateInternal()
    {
        _currentSpeed += _param.Accel * CpTime.DeltaTime;
        _currentAngleSpeed += _param.AngleAccel * CpTime.DeltaTime;

        _currentDegree += _currentAngleSpeed * CpTime.DeltaTime;
    }

    public override void GetPosValue(out ECpMoverPositionType outPosType, out Vector2 outValue)
    {
        outPosType = ECpMoverPositionType.Velocity;

        Vector2 direction = SltMath.ToVector(_currentDegree);
        outValue = direction * _currentSpeed;
    }

    public override void GetYawValue(out ECpMoverRotationType outYawType, out float outValue)
    {
        outYawType = ECpMoverRotationType.NoYaw;
        outValue = 0f;
    }

    // çXêVópïœêî
    float _currentDegree = 0f;
    float _currentSpeed = 0f;
    float _currentAngleSpeed = 0f;

    //
    FCpMoveParamEnemyShotDefault _param;
}
