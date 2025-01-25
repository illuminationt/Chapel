using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void Reset()
    {
        this = new FCpMoverContext();
    }
    public CpMoverManager OwnerMoverManager;
    public CpActorBase OwnerActor;
    public Transform OwnerTransform => OwnerActor.transform;
    public Vector2 InitialVelocity;
    public Vector2 InitialOwnerPosition;
    public float InitialOwnerDegree;
}

