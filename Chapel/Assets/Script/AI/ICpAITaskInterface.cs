using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public interface ICpTaskInterface
{
    protected abstract CpTaskComponent GetTaskComponent();

    public void RequestStartAI()
    {
        CpTaskComponent taskComp = GetTaskComponent();


    }

    private void RequestStartScriptGraph()
    {
        CpTaskComponent taskComp = GetTaskComponent();

        // ScriptGraphがきちんと設定されてるかチェック
#if UNITY_EDITOR
        ScriptMachine Machine = taskComp.GetComponent<ScriptMachine>();
        if (Machine.nest.macro == null)
        {
            //CtdDebug.LogError(name + "にScriptGraphが設定されてません");
            Assert.IsTrue(false);
            return;
        }
#endif

    }

    private void RequestStartStateMachine()
    {

    }
}
