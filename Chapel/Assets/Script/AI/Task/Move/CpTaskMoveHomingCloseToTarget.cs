using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CpTaskMoveHomingCloseToTarget : CpTaskBase
{
    public CpTaskMoveHomingCloseToTarget(in FCpMoveParamHomingCloseToTarget param)
    {
        _param = param;
    }

    protected override void OnStartInternal()
    {
        CpMoveComponent moveComp = Owner.GetComponent<CpMoveComponent>();
        FCpMoverId _moverId = moveComp.RequestStart(_param);
        moveComp.OnMoveFinished.AddListener(OnMoveFinished);
    }

    protected override void UpdateInternal(float DeltaTime)
    {

    }

    protected override void OnFinishInternal(ESltEndTaskReason endReason)
    {

    }

    void OnMoveFinished(FCpMoverId moverId)
    {
        if (_moverId.Equals(moverId))
        {
            EndTask(ESltEndTaskReason.FinishTask);
        }
    }
    FCpMoveParamHomingCloseToTarget _param;
    FCpMoverId _moverId;
}

[UnitTitle("CpTaskMoveHomingCloseToTarget")]
[UnitCategory("Cp/Move")]
[UnitSubtitle("CpTaskMoveHomingCloseToTarget")]
public class CpUnit_CpTaskMoveHomingCloseToTarget : CpUnitBase
{
    ValueInput inputParam;
    FCpMoveParamHomingCloseToTarget param;


    protected override SltTaskBase CreateTask()
    {
        return new CpTaskMoveHomingCloseToTarget(param);
    }

    protected override void InitializeUnitVariables(Flow flow)
    {
        // example: vectorValue = flow.GetValue<Vector2>(inputVectorValue);
        param = flow.GetValue<FCpMoveParamHomingCloseToTarget>(inputParam);
    }

    protected override void DefinitionInternal()
    {
        // exapmple: inputVectorValue = ValueInput("VectorValue", Vector2.zero);
        inputParam = ValueInput("Param", new FCpMoveParamHomingCloseToTarget());
    }
}