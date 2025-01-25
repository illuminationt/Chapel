using UnityEngine;
using Unity.VisualScripting;
using NUnit.Framework;
using Unity.Mathematics;


[Inspectable]
[System.Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamRotateInfToTarget : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() { return ECpMoveParamType.RotateInfToTarget; }

    [Inspectable] public ECpRotateTargetType TargetType;
    [Inspectable] public float InitialSpeed;
    [Inspectable] public float MaxSpeed;
    [Inspectable] public float Accel;
    [Inspectable] public float AngleTolerance;
}

public class CpMoverRotateInfinitelyToTarget : CpMoverBase
{
    public static CpMoverRotateInfinitelyToTarget Create(in FCpMoveParamRotateInfToTarget param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverRotateInfinitelyToTarget>(context);
        mover._param = param;
        return mover;
    }
    protected override void InitializeInternal()
    {
        _currentDegree = Context.InitialOwnerDegree;
        _currentSpeed = _param.InitialSpeed;
    }

    protected override void UpdateInternal()
    {
        _currentSpeed += _param.Accel * CpTime.DeltaTime;
        if (_currentSpeed > _param.MaxSpeed)
        {
            _currentSpeed = _param.MaxSpeed;
        }

        // ñ⁄ïWï˚å¸Ç…å¸ÇØÇÈ
        Vector2 selfDirection = Context.OwnerTransform.GetForwardVector();

        Vector2 selfPosition = Context.OwnerTransform.position;
        Vector2 targetPosition = Vector2.zero;
        bool bExistsTarget = CpRotateTargetUtil.GetTargetLocation(_param.TargetType, ref targetPosition);
        Vector2 toTargetDirection = Vector2.zero;
        if (bExistsTarget)
        {
            toTargetDirection = (targetPosition - selfPosition).normalized;
        }
        else
        {
            toTargetDirection = selfDirection;
        }


        float angleSub = selfDirection.GetDegreeTo(toTargetDirection);
        if (Mathf.Abs(angleSub) < _param.AngleTolerance)
        {
            _latestDeltaDegree = 0f;
            return;
        }
        bool bClockwise = selfDirection.IsClockwise(toTargetDirection);
        float angleSign = bClockwise ? -1f : 1f;
        float deltaAngle = angleSign * _currentSpeed * CpTime.DeltaTime;
        if (Mathf.Abs(deltaAngle) > angleSub)
        {
            // äpìxí¥Ç¶ÇΩéûÇÃèàóù
            deltaAngle = Mathf.Sign(deltaAngle) * angleSub;
        }


        _latestDeltaDegree = deltaAngle;
    }


    public override void GetPosValue(out ECpMoverPositionType outPosType, out Vector2 outValue)
    {
        outPosType = ECpMoverPositionType.NoPos;
        outValue = Vector2.zero;
    }
    public override void GetYawValue(out ECpMoverRotationType outYawType, out float outValue)
    {
        outYawType = ECpMoverRotationType.DeltaYaw;
        outValue = _latestDeltaDegree;
    }

    float _currentDegree = 0f;
    float _currentSpeed = 0f;

    float _latestDeltaDegree = 0f;

    FCpMoveParamRotateInfToTarget _param;
}
