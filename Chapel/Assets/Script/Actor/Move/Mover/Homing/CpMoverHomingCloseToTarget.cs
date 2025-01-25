using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

[Inspectable]
[System.Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamHomingCloseToTarget : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.HomingCloseToTarget;
    [Inspectable]
    public ECpHomingTarget Target;
    [Inspectable]
    public float MaxSpeed;
    [Inspectable]
    public float Accel;
}

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
        _newVelocity = GetCurrentOwnerVelocity();
    }

    protected override void UpdateInternal()
    {
        Vector2 currentVelocity = GetCurrentOwnerVelocity();

        Vector2 targetPosition = CpHomingUtil.CalcHomingTargetLocation(_param.Target);
        Vector2 myPosition = GetCurrentOwnerActorPosition();
        Vector2 toTarget = (targetPosition - myPosition).normalized;

        //SltDebugDraw.DrawArrow(myPosition, myPosition + toTarget * 20, Color.green, 1);
        _newVelocity = currentVelocity + toTarget * _param.Accel * CpTime.DeltaTime;
        //SltDebugDraw.DrawArrow(myPosition, toTarget.normalized, 50, Color.red, 2);

        if (_newVelocity.magnitude > _param.MaxSpeed)
        {
            _newVelocity = _newVelocity.normalized * _param.MaxSpeed;
        }
    }

    public override void GetPosValue(out ECpMoverPositionType outPosType, out Vector2 outValue)
    {
        outPosType = ECpMoverPositionType.Velocity;
        outValue = _newVelocity;
    }
    public override void GetYawValue(out ECpMoverRotationType outYawType, out float outValue)
    {
        outYawType = ECpMoverRotationType.NoYaw;
        outValue = 0f;
    }

    Vector2 _newVelocity;
    FCpMoveParamHomingCloseToTarget _param;

#if CP_DEBUG
    public override void DrawImGui()
    {
        base.DrawImGui();
    }
#endif
}
