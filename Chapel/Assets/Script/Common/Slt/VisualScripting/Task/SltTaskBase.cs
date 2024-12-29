using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ESltUnitWaitEndType
{
    NoWait,
    Seconds,
    UntilEndTask,
}

public enum ESltEndTaskReason
{
    FinishTask,
    TimeExpired,
    External,
}

public struct FSltUnitWaitEndParam
{
    public ESltUnitWaitEndType Type;

    // 待ち時間（waitTypeがSecondsの場合のみ使用）
    public float Time;
}

[System.Serializable]
public class SltTaskBase
{
    public GameObject Owner = null;
    public SltTaskComponent OwnerTaskComponent = null;
    public FSltUnitWaitEndParam WaitParam;

    // タスク開始時（Defaultノードへ進む）
    public UnityEvent OnTaskStart = new UnityEvent();
    // タスク終了時(実行成功)
    public UnityEvent OnFinishAction = new UnityEvent();
    // タスク終了時（外部から終了させられた）
    public UnityEvent OnFinishExternal = new UnityEvent();

    // Update()を実行するフラグ
    public bool bShouldUpdate = false;

    // VisualScripting上のノードのGUID
    public System.Guid UnitGuid;

    // 終了済みフラグ
    public bool IsFinished = false;

    // ノード入ったときに呼ばれる。
    public void OnStart()
    {
        OnTaskStart.Invoke();
        OnStartInternal();
    }
    protected virtual void OnStartInternal() { }

    // ノードで毎フレーム呼ぶ
    public void Update(float DeltaTime)
    {
        if (WaitParam.Type == ESltUnitWaitEndType.Seconds)
        {
            if (WaitParam.Time > 0f)
            {
                WaitParam.Time -= DeltaTime;
                if (WaitParam.Time < 0f)
                {
                    EndTask(ESltEndTaskReason.TimeExpired);
                }
            }
        }
        UpdateInternal(DeltaTime);
    }
    protected virtual void UpdateInternal(float DeltaTime) { }

    protected void OnFinish(ESltEndTaskReason endReason)
    {
        OnFinishInternal(endReason);
    }
    protected virtual void OnFinishInternal(ESltEndTaskReason endReason) { }

    protected void EndTask(ESltEndTaskReason endReason)
    {
        if (IsFinished)
        {
            return;
        }
        IsFinished = true;
        OwnerTaskComponent.RegisterRemoveTask(this);
        OnFinish(endReason);

        switch (endReason)
        {
            // CtdBoltUnit_Base のOutput呼び出し関数を呼ぶ
            case ESltEndTaskReason.FinishTask:
                OnFinishAction.Invoke();
                break;

            case ESltEndTaskReason.External:
                OnFinishExternal.Invoke();
                break;
        }
    }
}
