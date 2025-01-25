using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CpTaskMoveOmni : CpTaskBase
{
    public override ECpTaskType GetTaskType() { return ECpTaskType.Move; }
    public CpTaskMoveOmni(ICpMoveParam moveParam)
    {
        _moveParam = moveParam;
    }

    protected override void OnStartInternal()
    {
        CpMoveComponent moveComp = Owner.GetComponent<CpMoveComponent>();
        _moverId = moveComp.RequestStart(_moveParam);

        moveComp.OnMoveFinished.AddListener(OnMoveFinished);
    }

    protected override void UpdateInternal(float DeltaTime)
    {

    }

    protected override void OnFinishInternal(ESltEndTaskReason endReason)
    {
        CpMoveComponent moveComp = Owner.GetComponent<CpMoveComponent>();
        moveComp.OnMoveFinished.RemoveListener(OnMoveFinished);
    }

    void OnMoveFinished(FCpMoverId moverId)
    {
        if (moverId.Equals(_moverId))
        {
            EndTask(ESltEndTaskReason.FinishTask);
        }
    }
    ICpMoveParam _moveParam;
    FCpMoverId _moverId;
}

[UnitTitle("CpTaskMoveOmni")]
[UnitCategory("Cp/Move")]
[UnitSubtitle("CpTaskMoveOmni")]
public class CpUnit_CpTaskMoveOmni : CpUnitBase
{
    ValueInput inputMoveParam;
    ICpMoveParam moveParam;

    protected override SltTaskBase CreateTask(GameObject ownerObj)
    {
        return new CpTaskMoveOmni(moveParam);
    }

    protected override void InitializeUnitVariables(Flow flow)
    {
        // example: vectorValue = flow.GetValue<Vector2>(inputVectorValue);

        moveParam = flow.GetValue<ICpMoveParam>(inputMoveParam);

    }

    protected override void DefinitionInternal()
    {
        // exapmple: inputVectorValue = ValueInput("VectorValue", Vector2.zero);

        inputMoveParam = ValueInput<ICpMoveParam>("MoveParam", null);
    }
}