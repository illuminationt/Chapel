using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ECpHomingType
{
    [InspectorName("��葬�x.��]�̂݉\")]
    OnlyRotate = 0,// ��葬�x.��]�������ăz�[�~���O����

    // ���ݑ��x�Ƀ^�[�Q�b�g�����̈ړ��ʂ�������z�[�~���O
    // �������́A�����̕ω���K�������Ӗ����Ȃ��B�������ς������A�������������Ȃ邱�Ƃ������Ƃ���.
    [InspectorName("��Ƀ^�[�Q�b�g�ɋ߂Â������ɉ���(��)")]
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