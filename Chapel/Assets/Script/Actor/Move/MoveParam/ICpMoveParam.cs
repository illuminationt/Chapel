using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ECpMoveParamType
{
    None,
    Linear = 1,
    Curve = 2,
    Tween = 3,

    EnemyShotDefault = 100,
    EnemyShotMoveParam = 101,
    EnemyShotMoveParamList = 102,

    HomingOnlyRotate = 500,
    HomingCloseToTarget = 501,
}
public interface ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType();
}
