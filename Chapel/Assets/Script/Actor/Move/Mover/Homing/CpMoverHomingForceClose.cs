using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

/**
 * 初速方向から目標方向へとだんだんと補間していく。
 * InterpDuration秒経つと、速度は完全に目標方向で上書き。
 * イメージは、現在方向 = (1-t)*初速方向 + t*目標方向
 * InterpDuraion秒経つと、t=1になる
 */
[Inspectable]
[System.Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamHomingForceClose : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.HomingForceClose;
    [Inspectable]
    public ECpHomingTarget Target;
    [Inspectable]
    public float MaxSpeed;
    [Inspectable]
    public float Accel;
    [Inspectable]
    public float InterpDuration;
}

public class CpMoverHomingForceClose : CpMoverBase
{
    public static CpMoverHomingForceClose Create(in FCpMoveParamHomingForceClose param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverHomingForceClose>(context);
        mover._param = param;
        return mover;
    }
    protected override void InitializeInternal()
    {
        _newVelocity = GetCurrentOwnerVelocity();
        _initialDirection = _newVelocity.normalized;
    }

    protected override void UpdateInternal()
    {
        Vector2 currentVelocity = GetCurrentOwnerVelocity();
        float currentSpeed = currentVelocity.magnitude;
        float newSpeed = currentSpeed + _param.Accel * CpTime.DeltaTime;
        if (newSpeed > _param.MaxSpeed)
        {
            newSpeed = _param.MaxSpeed;
        }

        Vector2 targetPosition = CpHomingUtil.CalcHomingTargetLocation(_param.Target);
        Vector2 myPosition = GetCurrentOwnerActorPosition();
        Vector2 toTarget = (targetPosition - myPosition).normalized;

        Assert.IsTrue(_param.InterpDuration > 0, $"ForceCloseHomingのInterpDurationがゼロです");
        float currentAlpha = Mathf.Min(Duration / _param.InterpDuration, 1f);
        Vector2 newDirection = (1f - currentAlpha) * _initialDirection + currentAlpha * toTarget;

        _newVelocity = newDirection * newSpeed;
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
    Vector2 _initialDirection = Vector2.zero;
    FCpMoveParamHomingForceClose _param;

#if CP_DEBUG
    public override void DrawImGui()
    {
        base.DrawImGui();
    }
#endif
}
