using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;


[RequireComponent(typeof(CpHellComponent))]
public class CpHellTestObject : CpActorBase
{
    CpHellComponent _hellComponent = null;

    protected override void Awake()
    {
        base.Awake();
        _hellComponent = GetComponent<CpHellComponent>();
    }

    // CpActorBase interface
    public override ECpMoverUpdateType GetMoverUpdateType() { return ECpMoverUpdateType.UpdateFunction; }

    // end of CpActorBase interface

    // ICpActorForwardInterface
    public override float GetForwardDegree()
    {
        return SltMath.ToDegree(Vector2.down);
    }

    // end of ICpActorForwardInterface


    public void RequestStartHell(CpHellParamScriptableObject paramSO)
    {
        _hellComponent.RequestStart(paramSO.MultiHellParam, null);
    }
    public void RequestStartHell(CpHellParamListScriptableObject paramSO)
    {
        List<FCpMultiHellUpdatorId> idList = null;
        _hellComponent.RequestStart(paramSO.HellParamList, out idList);
    }
}
