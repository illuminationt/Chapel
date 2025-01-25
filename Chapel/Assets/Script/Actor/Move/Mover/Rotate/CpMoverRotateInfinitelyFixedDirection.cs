using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Unity.VisualScripting;
using NUnit.Framework;
using System;

public enum ECpRotateInfinitelyType
{
    None = 0,
    FixedDirection = 1,// “¯‚¶•ûŒü‚É‰ñ“]‚µ‘±‚¯‚é
    ToTarget,// –Ú•W•ûŒü‚ÉŒü‚©‚Á‚Ä‰ñ“]‚µ‘±‚¯‚é
}

[Inspectable]
[System.Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamRotateInfFixedDirection : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() { return ECpMoveParamType.RotateInfFixedDirection; }

    [Inspectable] public bool bClockwise;
    [Inspectable] public float InitialSpeed;
    [Inspectable] public float MaxSpeed;
    [Inspectable] public float Accel;
}

// –¾¦“I‚ÉI—¹‚³‚¹‚ç‚ê‚é‚½‚ß‰i‰“‚É‰ñ“]
public class CpMoverRotateInfinitelyFixedDirection : CpMoverBase
{
    public static CpMoverRotateInfinitelyFixedDirection Create(in FCpMoveParamRotateInfFixedDirection param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverRotateInfinitelyFixedDirection>(context);
        mover._param = param;
        return mover;
    }
    protected override void InitializeInternal()
    {
        _currentSpeed = _param.InitialSpeed;
        _currentSpeed *= _param.bClockwise ? -1f : 1f;
    }
    protected override void UpdateInternal()
    {
        float accel = _param.Accel * (_param.bClockwise ? -1f : 1f);
        _currentSpeed += accel * CpTime.DeltaTime;
        if (Mathf.Abs(_currentSpeed) > _param.MaxSpeed)
        {
            _currentSpeed = Mathf.Sign(_currentSpeed) * _param.MaxSpeed;
        }
    }
    public override void GetPosValue(out ECpMoverPositionType outPosType, out Vector2 outValue)
    {
        outPosType = ECpMoverPositionType.Velocity;
        outValue = Vector2.zero;
    }
    public override void GetYawValue(out ECpMoverRotationType outYawType, out float outValue)
    {
        outYawType = ECpMoverRotationType.RotationSpeed;
        outValue = _currentSpeed;
    }

    FCpMoveParamRotateInfFixedDirection _param;
    float _currentSpeed = 0f;
}
