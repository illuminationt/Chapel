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

    protected override void OnStartInternal()
    {
        CpMoveComponent moveComp = Owner.GetComponent<CpMoveComponent>();
        moveComp.RequestStart(_moveParamLinearSO.GetMoveParam());
    }
}

[UnitTitle("CpMoveLinear")]
[UnitCategory("Cp/Move")]
[UnitSubtitle("CpTaskMoveLinear")]
public class CpUnit_MoveLinear : CpUnitBase
{
    ValueInput inputMoveParamSO;
    CpMoveParamLinearScriptableObject moveParamSO;

    protected override SltTaskBase CreateTask()
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