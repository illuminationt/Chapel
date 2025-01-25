using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class CpTask_Wait : CpTaskBase
{
    float WaitTime;
    public CpTask_Wait(float inWaitTime)
    {
        WaitTime = inWaitTime;
        bShouldUpdate = true;
        //SltDebug.Log("SltTask_Wait:Start");
    }

    public override ECpTaskType GetTaskType()
    {
        return ECpTaskType.Wait;
    }

    protected override void UpdateInternal(float DeltaTime)
    {
        if (WaitTime > 0f)
        {
            WaitTime -= DeltaTime;
            if (WaitTime < 0f)
            {
                EndTask(ESltEndTaskReason.FinishTask);
            }
        }
    }

    protected override void OnFinishInternal(ESltEndTaskReason EndReason)
    {
        WaitTime = 0f;
    }
}


// ˆê’èŽžŠÔ‘Ò‚Â
[UnitTitle("CpTaskWait")]
[UnitCategory("Cp/Wait")]
[UnitSubtitle("CpTask_Wait.cs")]
public class CpBoltUnit_Wait : CpUnitBase
{
    ValueInput inputWaitTime;
    float WaitTime;

    protected override SltTaskBase CreateTask(GameObject ownerObj)
    {
        return new CpTask_Wait(WaitTime);
    }

    protected override void InitializeUnitVariables(Flow flow)
    {
        WaitTime = flow.GetValue<float>(inputWaitTime);
    }

    protected override void DefinitionInternal()
    {
        inputWaitTime = ValueInput<float>("WaitTime", 0f);
    }
}
