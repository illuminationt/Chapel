using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;


public enum ECpHomingType
{
    [InspectorName("一定速度.回転のみ可能")]
    OnlyRotate = 0,// 一定速度.回転だけしてホーミングする

    // 現在速度にターゲット方向の移動量を加えるホーミング
    // ※加速は、速さの変化を必ずしも意味しない。方向が変わったり、速さが小さくなることも加速という.
    [InspectorName("常にターゲットに近づく方向に加速(※)")]
    CloseToTarget = 1,
}

public enum ECpHomingTarget
{
    Player = 0,
}

public enum ECpHomingChangeTurnDirectionType
{
    Quickly,
    Delay_ContinuePrevTurn,
    Delay_StopPrevTurn,
}

[Inspectable]
[System.Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamHomingOnlyRotate : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.HomingOnlyRotate;

    public ECpHomingTarget Target;

    public float Speed;
    public SltFloatInterval SpeedMinMax;
    public float Accel;

    public float AngleSpeed;
    ECpHomingChangeTurnDirectionType ChangeTurnDirectionType;
    public float MaxTurnableAngle;
}



public class CpMoverHomingOnlyRotate : CpMoverBase
{
    public static CpMoverHomingOnlyRotate Create(in FCpMoveParamHomingOnlyRotate param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverHomingOnlyRotate>(context);
        mover._param = param;
        return mover;
    }

    protected override void InitializeInternal()
    {
        _currentSpeed = _param.Speed;
        _currentDegree = Context.InitialOwnerDegree;
    }

    protected override void UpdateInternal()
    {
        _currentSpeed += _param.Accel * CpTime.DeltaTime;

        Vector2 targetPosition = CalcHomingTargetLocation();
        Vector2 myPosition = Context.OwnerActor.transform.position;
    }


    public override void GetPosValue(out ECpMoverPositionType outPosType, out Vector2 outValue)
    {
        outPosType = ECpMoverPositionType.NoPos;
        outValue = Vector2.zero;
    }
    public override void GetYawValue(out ECpMoverRotationType outYawType, out float outValue)
    {
        outYawType = ECpMoverRotationType.NoYaw;
        outValue = 0f;
    }

    Vector2 CalcHomingTargetLocation()
    {
        switch (_param.Target)
        {
            case ECpHomingTarget.Player:
                return CpUtil.GetPlayerWorldPosition();
            default:
                Assert.IsTrue(false);
                return Vector2.zero;
        }
    }

    float _currentSpeed;
    float _currentDegree;

    FCpMoveParamHomingOnlyRotate _param;
}
