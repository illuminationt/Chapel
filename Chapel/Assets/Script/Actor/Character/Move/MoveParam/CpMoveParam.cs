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

    [SerializeField]
    public FCpMoveParamTween MoveParamTween;
}

// ScriptableObject���Őݒ肷��l
[System.Serializable]
public abstract class CpMoveParamBase
{
}

// �Q�[�����̐i�s�󋵂ɉ�����Mover�ɓn�������l
public struct FCpMoverContext
{
    public CpActorBase OwnerActor;
    public Vector2 Velocity;
    public Vector2 OwnerPosition;
    public float OwnerDegree;
}

