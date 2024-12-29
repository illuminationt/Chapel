using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CpTaskComponent))]
public class CpEnemyBase : CpActorBase
{
    public void Start()
    {
        CpTaskComponent taskComp = GetComponent<CpTaskComponent>();
        taskComp.StartStateMachine();
    }
}
