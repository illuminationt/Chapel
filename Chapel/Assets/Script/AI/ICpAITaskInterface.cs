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

        // ScriptGraph��������Ɛݒ肳��Ă邩�`�F�b�N
#if UNITY_EDITOR
        ScriptMachine Machine = taskComp.GetComponent<ScriptMachine>();
        if (Machine.nest.macro == null)
        {
            //CtdDebug.LogError(name + "��ScriptGraph���ݒ肳��Ă܂���");
            Assert.IsTrue(false);
            return;
        }
#endif

    }

    private void RequestStartStateMachine()
    {

    }
}
