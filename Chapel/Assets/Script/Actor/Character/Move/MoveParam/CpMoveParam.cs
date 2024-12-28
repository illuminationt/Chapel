using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ECpMoveParamType
{
    None,
    Linear = 1,
    Curve = 2,
}

[System.Serializable]
public struct FCpMoveParam
{
    [SerializeField]
    public ECpMoveParamType MoveParamType;

    [SerializeField]
    public FCpMoveParamLinear MoveParamLinear;

    [SerializeField]
    public FCpMoveParamCurve MoveParamCurve;
}

// ScriptableObject���Őݒ肷��l
[System.Serializable]
public abstract class CpMoveParamBase
{
}

// �Q�[�����̐i�s�󋵂ɉ�����Mover�ɓn�������l
public struct FCpMoverContext
{
    public Vector2 Velocity;
    public float OwnerDegree;
}

