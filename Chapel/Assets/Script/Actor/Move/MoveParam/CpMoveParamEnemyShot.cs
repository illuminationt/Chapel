using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
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

[System.Serializable]
public struct FCpMoveParamEnemyShotMoveParam : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.EnemyShotMoveParam;

    public int Dummy;
}

[System.Serializable]
public struct FCpMoveParamEnemyShotMoveParamList : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.EnemyShotMoveParamList;

    public float a;
    public FCpMoveParamEnemyShotMoveParam param2;
    public FCpMoveParamEnemyShotDefault basdu;
    int souenf;
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
