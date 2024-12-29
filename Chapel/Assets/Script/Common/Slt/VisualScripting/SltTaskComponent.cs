using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

[DisallowMultipleComponent]
[RequireComponent(typeof(StateMachine))]
public class SltTaskComponent : MonoBehaviour
{
    [HideInInspector]
    public List<SltTaskBase> ActiveTasks = new List<SltTaskBase>();
    // Updateのforループ内で新しく生成されたタスクを一時保存する配列.
    [HideInInspector]
    public List<SltTaskBase> PendingActiveTasks = new List<SltTaskBase>();
    // タスクをまとめて削除するために一時保存する配列.
    [HideInInspector]
    public List<SltTaskBase> PendingRemoveTasks = new List<SltTaskBase>();

    // 既に開始済みフラグ
    bool bAlreadyStart = false;


    private void Update()
    {
        float DeltaTime = Time.deltaTime;
        for (int Index = ActiveTasks.Count - 1; Index >= 0; Index--)
        {
            SltTaskBase Task = ActiveTasks[Index];

            // ExternalCancelされた場合に備えてFinishチェック
            if (!Task.IsFinished)
            {
                ActiveTasks[Index].Update(DeltaTime);
            }
            if (ActiveTasks[Index].IsFinished)
            {
                ActiveTasks.RemoveAt(Index);
            }
        }

        // ↑のforループ内で生成されたTaskをActiveTask列に追加
        // Listの結合：https://www.delftstack.com/ja/howto/csharp/how-to-join-two-lists-together-in-csharp/
        ActiveTasks.AddRange(PendingActiveTasks);
        PendingActiveTasks.Clear();
    }

    public void RegisterTask(SltTaskBase newTask)
    {
        if (newTask == null)
        {
            Assert.IsTrue(false);
            return;
        }

        // 同じUnitから、１フレームに複数回タスクが発行されるのを防ぐ(UnitのGUIDを使って判定する）
        bool bSameGUID = PendingActiveTasks.Any(x => x.UnitGuid == newTask.UnitGuid);
        if (bSameGUID)
        {
            Assert.IsTrue(false);
            return;
        }

        newTask.Owner = gameObject;
        newTask.OwnerTaskComponent = this;

        // タスク開始
        PendingActiveTasks.Add(newTask);
        newTask.OnStart();
    }

    public void RegisterRemoveTask(SltTaskBase RemovedTask)
    {
        PendingRemoveTasks.Add(RemovedTask);
    }

    public void StartStateMachine()
    {
        if (bAlreadyStart)
        {
            Debug.LogError("既に開始済み");
            return;
        }
        StateMachine stateMachine = GetComponent<StateMachine>();

        // StateMachineがきちんと設定されてるかチェック
#if UNITY_EDITOR
        if (stateMachine.nest.macro == null)
        {
            Debug.LogError("StateMachine graph is null");
        }
#endif
        stateMachine.enabled = true;
        bAlreadyStart = true;
    }
}
