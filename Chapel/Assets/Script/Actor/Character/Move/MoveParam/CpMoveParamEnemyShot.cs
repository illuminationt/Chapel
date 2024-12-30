using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public struct FCpMoveParamEnemyShotDefault
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

    public float Speed;
    public SltFloatInterval SpeedInterval;
    public float Accel;
    public float AngleSpeed;
    public float AngleAccel;
    public Vector2 Direction;
}

public struct FCpMoveParamEnemyShotMoveParam
{

}

public struct FCpMoveParamEnemyShotMoveParamList
{

}

public struct FCpMoveParamEnemyShot
{
    public FCpMoveParamEnemyShot(bool bDummy = false)
    {
        MoveType = ECpEnemyShotMoveType.Default;
        ParamDefault = default;
        ParamMoveParam = default;
        ParamMoveParamList = default;
    }
    public ECpEnemyShotMoveType MoveType;
    public FCpMoveParamEnemyShotDefault ParamDefault;
    public FCpMoveParamEnemyShotMoveParam ParamMoveParam;
    public FCpMoveParamEnemyShotMoveParamList ParamMoveParamList;
}
public class CpMoveParamEnemyShot : CpMoveParamBase
{

}
