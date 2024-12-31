using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override Vector2 GetVelocity()
    {
        Vector2 direction = SltMath.ToVector(_currentDegree);
        return direction * _currentSpeed;
    }

    // çXêVópïœêî
    float _currentDegree = 0f;
    float _currentSpeed = 0f;
    float _currentAngleSpeed = 0f;

    //
    FCpMoveParamEnemyShotDefault _param;
}
