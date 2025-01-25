using ImGuiNET;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CpTaskComponent : SltTaskComponent
{
    // 
    string _pendingNextState = null;
    // VisualScript‚©‚çŒÄ‚Ño‚·
    public void ExitState(string nextStateName)
    {
        _pendingNextState = nextStateName;
        CpCustomEventExitState.Trigger(gameObject, nextStateName);
    }

    protected override void OnPreStartNewTask(SltTaskBase newTask)
    {
        CpTaskBase cpNewTask = newTask as CpTaskBase;
        if (cpNewTask == null)
        {
            Assert.IsTrue(false);
        }
        ECpTaskType newTaskType = cpNewTask.GetTaskType();

        foreach (SltTaskBase task in ActiveTasks)
        {
            CpTaskBase cptask = task as CpTaskBase;
            Assert.IsTrue(cptask != null);
            bool bSameType = cptask.GetTaskType() == newTaskType;
            bool bCanExecSameTime = CanExecuteSameTime(cptask.GetTaskType());
            if (bSameType && !bCanExecSameTime && !cptask.IsFinished)
            {
                cptask.ExternalCanel();
            }
        }

    }
    bool CanExecuteSameTime(ECpTaskType taskType)
    {
        return taskType switch
        {
            ECpTaskType.Move => true,
            ECpTaskType.Rotate => false,
            ECpTaskType.Hell => true,
            ECpTaskType.Animation => true,
            ECpTaskType.Wait => true,
            _ => throw new System.NotImplementedException(),
        };
    }

    public void StopMoveTask()
    {
        StopTask(ECpTaskType.Move);
    }
    public void StopRotateTask()
    {
        StopTask(ECpTaskType.Rotate);
    }
    public void StopHellTask()
    {
        StopTask(ECpTaskType.Hell);
    }
    public void StopAllTasks()
    {
        ForEachTasks((CpTaskBase task) =>
        {
            task.ExternalCanel();
        });
    }

    void StopTask(ECpTaskType taskType)
    {
        ForEachTasks((CpTaskBase task) =>
        {
            if (task.GetTaskType() == taskType)
            {
                task.ExternalCanel();
            }
        });
    }

    void ForEachTasks(UnityAction<CpTaskBase> func)
    {
        for (int index = ActiveTasks.Count - 1; index >= 0; index--)
        {
            CpTaskBase task = ActiveTasks[index] as CpTaskBase;
            if (task == null)
            {
                Assert.IsTrue(false);
                continue;
            }
            func(task);
        }
    }

#if CP_DEBUG
    public void DrawImGui()
    {
        if (ImGui.TreeNode("Active Tasks"))
        {
            ForEachTasks((CpTaskBase task) =>
            {
                task.DrawImGui();
            });
            ImGui.TreePop();
        }
    }
#endif
}
