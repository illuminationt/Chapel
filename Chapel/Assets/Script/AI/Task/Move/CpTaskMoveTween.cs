using DG.Tweening;
using NUnit.Framework;
using System;
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
    public CpTaskMoveTween(GameObject ownerObj, CpMoveTweenTaskParamDirection param)
    {
        CpActorBase ownerActor = ownerObj.GetComponent<CpActorBase>();
        Assert.IsTrue(ownerActor != null);
        _moveParamTween.Inject(ownerActor, param);
    }

    public override ECpTaskType GetTaskType()
    {
        return ECpTaskType.Move;
    }

    protected override void OnStartInternal()
    {
        CpMoveComponent moveComp = Owner.GetComponent<CpMoveComponent>();
        _id = moveComp.RequestStart(_moveParamTween);
        moveComp.OnMoveFinished.AddListener(OnMoveFinished);
    }

    protected override void OnFinishInternal(ESltEndTaskReason endReason)
    {
        base.OnFinishInternal(endReason);

        CpMoveComponent moveComponent = Owner.GetComponent<CpMoveComponent>();
        moveComponent.OnMoveFinished.RemoveListener(OnMoveFinished);
    }

    void OnMoveFinished(FCpMoverId id)
    {
        if (_id.Equals(id))
        {
            EndTask(ESltEndTaskReason.FinishTask);
        }
    }

    FCpMoveParamTween _moveParamTween;
    FCpMoverId _id;
}

[UnitTitle("CpTaskMoveTween")]
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

    protected override SltTaskBase CreateTask(GameObject ownerObj)
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

public enum ECpMoveTweenTaskDirectionType
{
    None = 0,
    Player = 1,
    SelfDirection = 10,
}

// 方向を指定してTween移動するタスクパラメータ
[Inspectable, Serializable, IncludeInSettings(true)]
public class CpMoveTweenTaskParamDirection
{
    public Vector2 GetOffset(CpActorBase owner)
    {
        Vector2 dir = CalcDirection(owner);
        dir = dir.Rotate(OffsetYaw);

        return dir * Distance;
    }

    Vector2 CalcDirection(CpActorBase owner)
    {
        switch (DirType)
        {
            case ECpMoveTweenTaskDirectionType.Player:
                {
                    CpPlayer player = CpPlayer.Get();
                    Vector2 playerPos = player.transform.position;

                    Vector2 ownerPos = owner.transform.position;

                    return ownerPos.GetDirectionTo(playerPos);

                }

            case ECpMoveTweenTaskDirectionType.SelfDirection:
                {
                    float ownerYaw = owner.transform.eulerAngles.z;
                    return SltMath.ToVector(ownerYaw);
                }
            default:
                Assert.IsTrue(false);
                return Vector2.zero;
        }
    }

    [Inspectable, SerializeField] public ECpMoveTweenTaskDirectionType DirType = ECpMoveTweenTaskDirectionType.None;
    [Inspectable, SerializeField] public float OffsetYaw = 0f;
    [Inspectable, SerializeField] public float Distance = 0f;
    [Inspectable, SerializeField] public float Duration = 0f;
    [Inspectable, SerializeField] public Ease EasingType = Ease.Unset;
}


[UnitTitle("CpTaskMoveTween(Direction)")]
[UnitCategory("Cp/Move")]
[UnitSubtitle("CpTaskMoveTween")]
public class CpUnit_MoveTweenDirection : CpUnitBase
{
    protected ValueInput inputParam;
    CpMoveTweenTaskParamDirection param;

    protected override SltTaskBase CreateTask(GameObject ownerObj)
    {
        return new CpTaskMoveTween(ownerObj, param);
    }

    protected override void InitializeUnitVariables(Flow flow)
    {
        param = flow.GetValue<CpMoveTweenTaskParamDirection>(inputParam);
    }

    protected override void DefinitionInternal()
    {
        inputParam = ValueInput<CpMoveTweenTaskParamDirection>("Param", null);
    }
}