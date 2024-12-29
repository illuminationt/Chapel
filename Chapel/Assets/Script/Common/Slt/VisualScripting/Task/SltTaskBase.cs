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

    // �҂����ԁiwaitType��Seconds�̏ꍇ�̂ݎg�p�j
    public float Time;
}

[System.Serializable]
public class SltTaskBase
{
    public GameObject Owner = null;
    public SltTaskComponent OwnerTaskComponent = null;
    public FSltUnitWaitEndParam WaitParam;

    // �^�X�N�J�n���iDefault�m�[�h�֐i�ށj
    public UnityEvent OnTaskStart = new UnityEvent();
    // �^�X�N�I����(���s����)
    public UnityEvent OnFinishAction = new UnityEvent();
    // �^�X�N�I�����i�O������I��������ꂽ�j
    public UnityEvent OnFinishExternal = new UnityEvent();

    // Update()�����s����t���O
    public bool bShouldUpdate = false;

    // VisualScripting��̃m�[�h��GUID
    public System.Guid UnitGuid;

    // �I���ς݃t���O
    public bool IsFinished = false;

    // �m�[�h�������Ƃ��ɌĂ΂��B
    public void OnStart()
    {
        OnTaskStart.Invoke();
        OnStartInternal();
    }
    protected virtual void OnStartInternal() { }

    // �m�[�h�Ŗ��t���[���Ă�
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
            // CtdBoltUnit_Base ��Output�Ăяo���֐����Ă�
            case ESltEndTaskReason.FinishTask:
                OnFinishAction.Invoke();
                break;

            case ESltEndTaskReason.External:
                OnFinishExternal.Invoke();
                break;
        }
    }
}
