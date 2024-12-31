using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CpTaskHell : CpTaskBase
{
    CpHellParamScriptableObject _hellParamSO = null;
    public CpTaskHell(CpHellParamScriptableObject hellParamSO)
    {
        _hellParamSO = hellParamSO;
    }

    protected override void OnStartInternal()
    {
        CpHellComponent hellComp = Owner.GetComponent<CpHellComponent>();
        _hellUdpatorId = hellComp.RequestStart(_hellParamSO.MultiHellParam);
        hellComp.OnHellFinished.AddListener(OnHellFinished);
    }

    void OnHellFinished(FCpMultiHellUpdatorId id)
    {
        if (id.Equals(_hellUdpatorId))
        {
            EndTask(ESltEndTaskReason.FinishTask);
        }
    }

    FCpMultiHellUpdatorId _hellUdpatorId;
}

[UnitTitle("CpTaskHell")]
[UnitCategory("Cp/Shot")]
[UnitSubtitle("CpTaskHell")]
public class CpUnit_Hell : CpUnitBase
{
    ValueInput inputMoveParamSO;
    CpHellParamScriptableObject hellParamSO;

    protected override SltTaskBase CreateTask()
    {
        return new CpTaskHell(hellParamSO);
    }

    protected override void InitializeUnitVariables(Flow flow)
    {
        hellParamSO = flow.GetValue<CpHellParamScriptableObject>(inputMoveParamSO);
    }

    protected override void DefinitionInternal()
    {
        inputMoveParamSO = ValueInput<CpHellParamScriptableObject>("HellParam", null);
    }
}