using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class CpTaskMoveTween : CpTaskBase
{
    public CpTaskMoveTween(in FCpMoveParamTween paramTween)
    {
        _moveParamTween = paramTween;
    }

    protected override void OnStartInternal()
    {
        CpMoveComponent moveComp = Owner.GetComponent<CpMoveComponent>();
        moveComp.RequestStart(_moveParamTween);
    }

    FCpMoveParamTween _moveParamTween;
}

[UnitTitle("CpMoveTween")]
[UnitCategory("Cp/Move")]
[UnitSubtitle("CpTaskMoveTween")]
public class CpUnit_MoveTween : CpUnitBase
{
    protected ValueInput inputVectorValue;
    protected ValueInput inputMoveTime;
    protected ValueInput inputVectorType;
    protected ValueInput inputEasingType;
    protected ValueInput inputMoveParamTween;

    public Vector2 vectorValue;
    public float moveTime;
    public ECpVectorType vectorType;
    public Ease easingType;

    protected override SltTaskBase CreateTask()
    {
        FCpMoveParamTween paramTween = new FCpMoveParamTween(vectorValue, moveTime, vectorType, easingType);
        return new CpTaskMoveTween(paramTween);
    }

    protected override void InitializeUnitVariables(Flow flow)
    {
        vectorValue = flow.GetValue<Vector2>(inputVectorValue);
        moveTime = flow.GetValue<float>(inputMoveTime);
        vectorType = flow.GetValue<ECpVectorType>(inputVectorType);
        easingType = flow.GetValue<Ease>(inputEasingType);
    }

    protected override void DefinitionInternal()
    {
        inputVectorValue = ValueInput("VectorValue", Vector2.zero);
        inputMoveTime = ValueInput("MoveTime", 0f);
        inputVectorType = ValueInput("VectorType", ECpVectorType.OffsetScreenSpace);
        inputEasingType = ValueInput("EasingType", Ease.Linear);
    }
}