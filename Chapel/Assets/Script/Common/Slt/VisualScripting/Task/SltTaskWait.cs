using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class SltTask_Wait : SltTaskBase
{
    float WaitTime;
    public SltTask_Wait(float inWaitTime)
    {
        WaitTime = inWaitTime;
        bShouldUpdate = true;
        //SltDebug.Log("SltTask_Wait:Start");
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
[UnitTitle("SltTaskWait")]
[UnitCategory("Slt/Wait")]
[UnitSubtitle("SltTask_Wait.cs")]
public class SltBoltUnit_Wait : SltUnitBase
{
    ValueInput inputWaitTime;
    float WaitTime;

    protected override SltTaskBase CreateTask()
    {
        return new SltTask_Wait(WaitTime);
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
