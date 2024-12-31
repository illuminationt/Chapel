using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class CpCustomEventExitState : SltGameObjectCustomEventBase<string>
{
    protected override string GetEventName()
    {
        return CpCustomEventName.ExitState;
    }

    protected override void DefinitionInternal()
    {
        result = ValueOutput<string>(nameof(result));
    }

    public static void Trigger(GameObject Target, string Arg)
    {
        EventBus.Trigger(CpCustomEventName.ExitState, Target, Arg);
    }
}
