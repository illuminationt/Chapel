using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CpTaskRotate : CpTaskBase
{

    public CpTaskRotate(CpRotateTaskParamFixedYaw param)
    {
        _paramRotate.Inject(param);
    }

    public CpTaskRotate(CpRotateTaskParamDeltaYaw param)
    {
        _paramRotate.Inject(param);
    }
    public CpTaskRotate(CpRotateTaskParamFace param)
    {
        _paramRotate.Inject(param);
    }

    public CpTaskRotate(in FCpMoveParamRotate param)
    {
        _paramRotate = param;
    }

    public override ECpTaskType GetTaskType()
    {
        return ECpTaskType.Rotate;
    }

    protected override void OnStartInternal()
    {
        CpMoveComponent moveComp = Owner.GetComponent<CpMoveComponent>();
        _id = moveComp.RequestStart(_paramRotate);
        moveComp.OnMoveFinished.AddListener(OnMoveFinished);
    }

    protected override void UpdateInternal(float DeltaTime)
    {
        base.UpdateInternal(DeltaTime);
    }

    protected override void OnFinishInternal(ESltEndTaskReason endReason)
    {
        base.OnFinishInternal(endReason);

        CpMoveComponent moveComponent = Owner.GetComponent<CpMoveComponent>();
        moveComponent.OnMoveFinished.RemoveListener(OnMoveFinished);

        moveComponent.RequestStop(_id, ECpMoveStopReason.MoverFinished);
    }

    void OnMoveFinished(FCpMoverId id)
    {
        if (_id.Equals(id))
        {
            EndTask(ESltEndTaskReason.FinishTask);
        }
    }

    FCpMoveParamRotate _paramRotate;
    FCpMoverId _id;
}

[Inspectable, Serializable, IncludeInSettings(true)]
public class CpRotateTaskParamBase
{
    [Inspectable] public float Speed;
    [Inspectable] public Ease EasingType;
}

// FixedYaw関連
[Inspectable, Serializable, IncludeInSettings(true)]
public class CpRotateTaskParamFixedYaw : CpRotateTaskParamBase
{
    [Inspectable, SerializeField] public float GoalYaw;
}

[UnitTitle("CpTaskRotate(FixedYaw)")]
[UnitCategory("Cp/Rotate")]
[UnitSubtitle("CpTaskRotate")]
public class CpUnit_CpTaskRotateFixedYaw : CpUnitBase
{
    ValueInput inputParam;
    CpRotateTaskParamFixedYaw param;

    protected override SltTaskBase CreateTask(GameObject ownerObj)
    {
        return new CpTaskRotate(param);
    }
    protected override void InitializeUnitVariables(Flow flow)
    {
        // example: vectorValue = flow.GetValue<Vector2>(inputVectorValue);

        param = flow.GetValue<CpRotateTaskParamFixedYaw>(inputParam);
    }

    protected override void DefinitionInternal()
    {
        // exapmple: inputVectorValue = ValueInput("VectorValue", Vector2.zero);

        inputParam = ValueInput<CpRotateTaskParamFixedYaw>("Param", null);
    }
}

// DeltaYaw関連
[Inspectable, Serializable, IncludeInSettings(true)]
public class CpRotateTaskParamDeltaYaw : CpRotateTaskParamBase
{
    [Inspectable, SerializeField] public float DeltaYaw;
}

[UnitTitle("CpTaskRotateDeltaYaw)")]
[UnitCategory("Cp/Rotate")]
[UnitSubtitle("CpTaskRotate")]
public class CpUnit_CpTaskRotateDeltaYaw : CpUnitBase
{
    ValueInput inputParam;
    CpRotateTaskParamDeltaYaw param;

    protected override SltTaskBase CreateTask(GameObject ownerObj)
    {
        return new CpTaskRotate(param);
    }
    protected override void InitializeUnitVariables(Flow flow)
    {
        // example: vectorValue = flow.GetValue<Vector2>(inputVectorValue);

        param = flow.GetValue<CpRotateTaskParamDeltaYaw>(inputParam);

    }
    protected override void DefinitionInternal()
    {
        // exapmple: inputVectorValue = ValueInput("VectorValue", Vector2.zero);

        inputParam = ValueInput<CpRotateTaskParamDeltaYaw>("Param", null);
    }
}

// end of DeltaYaw


// Face関連

// DeltaYaw関連
[Inspectable, Serializable, IncludeInSettings(true)]
public class CpRotateTaskParamFace : CpRotateTaskParamBase
{
    [Inspectable, SerializeField] public ECpRotateTargetType TargetType;
    [Inspectable, SerializeField] public float OffsetYaw;
}

[UnitTitle("CpTaskRotate(Face)")]
[UnitCategory("Cp/Rotate")]
[UnitSubtitle("CpTaskRotate")]
public class CpUnit_CpTaskRotateFace : CpUnitBase
{
    ValueInput inputParam;
    CpRotateTaskParamFace param;

    protected override SltTaskBase CreateTask(GameObject ownerObj)
    {
        return new CpTaskRotate(param);
    }
    protected override void InitializeUnitVariables(Flow flow)
    {
        // example: vectorValue = flow.GetValue<Vector2>(inputVectorValue);

        param = flow.GetValue<CpRotateTaskParamFace>(inputParam);
    }
    protected override void DefinitionInternal()
    {
        // exapmple: inputVectorValue = ValueInput("VectorValue", Vector2.zero);

        inputParam = ValueInput<CpRotateTaskParamFace>("Param", null);
    }
}


// end of Face