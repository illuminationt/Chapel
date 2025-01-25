using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CpTaskMoveLinear : CpTaskBase
{
    CpMoveParamLinearScriptableObject _moveParamLinearSO = null;
    public CpTaskMoveLinear(CpMoveParamLinearScriptableObject moveParamLinearSO)
    {
        _moveParamLinearSO = moveParamLinearSO;
    }

    public override ECpTaskType GetTaskType()
    {
        return ECpTaskType.Move;
    }
    protected override void OnStartInternal()
    {
        CpMoveComponent moveComp = Owner.GetComponent<CpMoveComponent>();
        _id = moveComp.RequestStart(_moveParamLinearSO.GetMoveParam());
        moveComp.OnMoveFinished.AddListener(OnMoveFinished);
    }

    void OnMoveFinished(FCpMoverId id)
    {
        if (_id.Equals(id))
        {
            EndTask(ESltEndTaskReason.FinishTask);
        }
    }

    FCpMoverId _id;
}

[UnitTitle("CpTaskMoveLinear")]
[UnitCategory("Cp/Move")]
[UnitSubtitle("CpTaskMoveLinear")]
public class CpUnit_MoveLinear : CpUnitBase
{
    ValueInput inputMoveParamSO;
    CpMoveParamLinearScriptableObject moveParamSO;

    protected override SltTaskBase CreateTask(GameObject ownerObj)
    {
        return new CpTaskMoveLinear(moveParamSO);
    }

    protected override void InitializeUnitVariables(Flow flow)
    {
        moveParamSO = flow.GetValue<CpMoveParamLinearScriptableObject>(inputMoveParamSO);
    }

    protected override void DefinitionInternal()
    {
        inputMoveParamSO = ValueInput<CpMoveParamLinearScriptableObject>("MoveParamLinear", null);
    }
}