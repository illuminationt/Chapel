using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

public class CpMoveParamHoming : CpMoveParamBase
{
    public FCpMoveParamHoming ParamHoming;
}

[System.Serializable]
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

[System.Serializable]
public struct FCpMoveParamHoming
{
    public ECpHomingType HomingType;

    public FCpMoveParamHomingOnlyRotate paramOnlyRotate;
    public FCpMoveParamHomingCloseToTarget paramCloseToTarget;
}