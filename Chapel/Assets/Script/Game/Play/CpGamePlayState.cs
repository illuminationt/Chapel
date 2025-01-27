using ImGuiNET;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public static class CpGamePlayStateUtil
{
    public static T CreateState<T>(CpGamePlayManager owner, ECpGamePlayState state) where T : CpGamePlayStateBase
    {
        System.Type stateClass = CpGamePlayStateUtil.GetGamePlayStateClass(state);
        T newState = (T)Activator.CreateInstance(stateClass, owner);
        return newState;
    }

    public static Dictionary<ECpGamePlayState, System.Type> GamePlayStateDict = new Dictionary<ECpGamePlayState, System.Type>() {
        {ECpGamePlayState.Root,typeof(CpGamePlayStateRoot) },
        {ECpGamePlayState.EnterDungeon,typeof(CpGamePlayStateEnterFloor) },
        {ECpGamePlayState.RoomBattle,typeof(CpGamePlayStateRoomBattle) },
        {ECpGamePlayState.RoomExplore,typeof(CpGamePlayStateRoomExplore) },
        {ECpGamePlayState.RoomTransition,typeof(CpGamePlayStateRoomTransition) },
    };

    public static System.Type GetGamePlayStateClass(ECpGamePlayState state)
    {
        return GamePlayStateDict[state];
    }
}

public enum ECpGamePlayStateSeq
{
    None = 0,
    Start = 1,
    Update = 5,
    Finished = 10,
}

public abstract class CpGamePlayStateBase
{
    public CpGamePlayStateBase(CpGamePlayManager ownerManager)
    {
        _ownerGamePlayManager = ownerManager;
    }
    public abstract ECpGamePlayState GetGamePlayState();
    public abstract List<ECpGamePlayState> GetCanStackStates();
    public bool CanStackOn(CpGamePlayStateBase otherState)
    {
        if (otherState == null)
        {
            return false;
        }
        List<ECpGamePlayState> canStackStates = GetCanStackStates();
        ECpGamePlayState otherStateType = otherState.GetGamePlayState();
        if (!canStackStates.Contains(otherStateType))
        {
            return false;
        }

        return true;
    }
    // �����I�ɊJ�n
    public void ReadyForActivation()
    {
        SetState(ECpGamePlayStateSeq.Update);
    }

    public void Update()
    {
        Assert.IsTrue(_currentSeq != ECpGamePlayStateSeq.None, "ReadyForActivation()���Ă�łȂ�����");
        switch (_currentSeq)
        {
            case ECpGamePlayStateSeq.Update:
                UpdateInternal();
                break;

            case ECpGamePlayStateSeq.Finished:
                break;

            default:
                throw new System.NotImplementedException();
        }
    }

    // �X�e�[�g�̊O����X�e�[�g���I��������
    public void ExternalCancel()
    {
        FinishState();
    }

    void OnStart()
    {
        OnStartInternal();
    }
    protected virtual void OnStartInternal() { }

    // ���̃X�e�[�g���w�肹���ɃX�e�[�g���I��������
    protected void FinishState()
    {
        SetState(ECpGamePlayStateSeq.Finished);
    }

    // ���̃X�e�[�g���w�肵�ăX�e�[�g���I��������
    // �X�e�[�g�̃C���X�^���X��new����֐����w�肵�Ă�������
    protected void FinishStateBy(Func<CpGamePlayStateBase> CreateStateFunc)
    {

    }

    protected virtual void UpdateInternal() { }

    void OnFinished()
    {
        OnFinishedInternal();
    }
    protected virtual void OnFinishedInternal() { }

    public bool IsFinished()
    {
        return _currentSeq == ECpGamePlayStateSeq.Finished;
    }

    void SetState(ECpGamePlayStateSeq newState)
    {
        if (_currentSeq == newState)
        {
            return;
        }

        _currentSeq = newState;

        switch (newState)
        {
            case ECpGamePlayStateSeq.Update:
                // �X�V�����J�n���ɃX�e�[�g�J�n�������Ă�
                OnStart();
                _ownerGamePlayManager.InvokeGamePlayStateSeqChangedEvent(GetGamePlayState(), ECpGamePlayStateSeq.Start);
                _ownerGamePlayManager.InvokeGamePlayStateSeqChangedEvent(GetGamePlayState(), ECpGamePlayStateSeq.Update);
                break;

            case ECpGamePlayStateSeq.Finished:
                OnFinished();
                _ownerGamePlayManager.InvokeGamePlayStateSeqChangedEvent(GetGamePlayState(), ECpGamePlayStateSeq.Finished);
                break;
        }
    }
    public CpGamePlayManager OwnerGamePlayManager => _ownerGamePlayManager;
    CpGamePlayManager _ownerGamePlayManager = null;
    ECpGamePlayStateSeq _currentSeq = ECpGamePlayStateSeq.None;

#if CP_DEBUG
    public void DrawImGui()
    {
        string stateStr = $"StateType = {SltEnumUtil.ToString(GetGamePlayState())}";
        ImGui.Text(stateStr);

        string currentStateStr = $"CurrentState = {SltEnumUtil.ToString(_currentSeq)}";
        ImGui.Text(currentStateStr);

        DrawImGuiInternal();
    }

    protected virtual void DrawImGuiInternal()
    {

    }
#endif
}

// �������Ȃ��X�e�[�g
public class CpGamePlayStateRoot : CpGamePlayStateBase
{
    public CpGamePlayStateRoot(CpGamePlayManager owner) : base(owner) { }

    public override ECpGamePlayState GetGamePlayState()
    {
        return ECpGamePlayState.Root;
    }
    public override List<ECpGamePlayState> GetCanStackStates()
    {
        return null;
    }
}