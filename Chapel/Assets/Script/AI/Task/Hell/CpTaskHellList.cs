using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CpTaskHellList : CpTaskBase
{
    CpHellParamListScriptableObject _hellListSO = null;
    List<FCpMultiHellUpdatorId> _hellUpdatorIdList = null;
    public CpTaskHellList(CpHellParamListScriptableObject hellListSO)
    {
        _hellListSO = hellListSO;
    }
    public override ECpTaskType GetTaskType()
    {
        return ECpTaskType.Hell;
    }

    protected override void OnStartInternal()
    {
        CpHellComponent hellComp = Owner.GetComponent<CpHellComponent>();
        hellComp.RequestStart(_hellListSO.HellParamList, out _hellUpdatorIdList);
        hellComp.OnHellFinished.AddListener(OnHellFinished);
    }

    protected override void UpdateInternal(float DeltaTime)
    {

    }

    protected override void OnFinishInternal(ESltEndTaskReason endReason)
    {

    }

    void OnHellFinished(FCpMultiHellUpdatorId id)
    {
        for (int index = 0; index < _hellUpdatorIdList.Count; index++)
        {
            if (_hellUpdatorIdList[index].Equals(id))
            {
                _hellUpdatorIdList.RemoveAt(index);
                break;
            }
        }

        if (_hellUpdatorIdList.Count == 0)
        {
            EndTask(ESltEndTaskReason.FinishTask);
        }
    }
}


[UnitTitle("CpTaskHellList")]
[UnitCategory("Cp/Shot")]
[UnitSubtitle("CpTaskHell")]
public class CpUnit_HellList : CpUnitBase
{
    ValueInput inputMoveParamSO;
    CpHellParamListScriptableObject hellParamSO;

    protected override SltTaskBase CreateTask(GameObject ownerObj)
    {
        return new CpTaskHellList(hellParamSO);
    }

    protected override void InitializeUnitVariables(Flow flow)
    {
        hellParamSO = flow.GetValue<CpHellParamListScriptableObject>(inputMoveParamSO);
    }

    protected override void DefinitionInternal()
    {
        inputMoveParamSO = ValueInput<CpHellParamListScriptableObject>("HellParam", null);
    }
}