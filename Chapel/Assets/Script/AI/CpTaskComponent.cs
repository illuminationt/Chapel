using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpTaskComponent : SltTaskComponent
{

    // 
    string _pendingNextState = null;
    // VisualScript����Ăяo��
    public void ExitState(string nextStateName)
    {
        _pendingNextState = nextStateName;
        CpCustomEventExitState.Trigger(gameObject, nextStateName);
    }
}
