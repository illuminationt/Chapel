using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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
