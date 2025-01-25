using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CpTaskMove : CpTaskBase
{
    public CpTaskMove(CpMoveParamScriptableObject moveParamSO)
    {
        _moveParamSO = moveParamSO;
    }

    public override ECpTaskType GetTaskType()
    {
        return ECpTaskType.Move;
    }

    protected override void OnStartInternal()
    {
        CpMoveComponent moveComp = Owner.GetComponent<CpMoveComponent>();
        _id = moveComp.RequestStart(_moveParamSO.MoveParam);
        moveComp.OnMoveFinished.AddListener(OnMoveFinished);
    }
    void OnMoveFinished(FCpMoverId id)
    {
        if (_id.Equals(id))
        {
            EndTask(ESltEndTaskReason.FinishTask);
        }
    }

    CpMoveParamScriptableObject _moveParamSO = null;
    FCpMoverId _id;
}

[UnitTitle("CpTaskMove")]
[UnitCategory("Cp/Move")]
[UnitSubtitle("CpTaskMove")]
public class CpUnit_Move : CpUnitBase
{
    ValueInput inputMoveParamSO;
    CpMoveParamScriptableObject moveParamSO;

    protected override SltTaskBase CreateTask(GameObject ownerObj)
    {
        return new CpTaskMove(moveParamSO);
    }

    protected override void InitializeUnitVariables(Flow flow)
    {
        moveParamSO = flow.GetValue<CpMoveParamScriptableObject>(inputMoveParamSO);
    }

    protected override void DefinitionInternal()
    {
        inputMoveParamSO = ValueInput<CpMoveParamScriptableObject>("MoveParam", null);
    }
}