using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public static class CpGamePlayStateUtil
{
    public static T CreateState<T>(ECpGamePlayState state)
    {
        System.Type stateClass = CpGamePlayStateUtil.GetGamePlayStateClass(state);
        T newState = (T)Activator.CreateInstance(stateClass);
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

public abstract class CpGamePlayStateBase
{
    enum ECpState
    {
        None = 0,
        Update,
        Finished,
    }
    public CpGamePlayStateBase()
    {
        _ownerGamePlayManager = CpGamePlayManager.Get();
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
        SetState(ECpState.Update);
    }

    public void Update()
    {
        Assert.IsTrue(_currentState != ECpState.None, "ReadyForActivation()���Ă�łȂ�����");
        switch (_currentState)
        {
            case ECpState.Update:
                UpdateInternal();
                break;

            case ECpState.Finished:
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
        SetState(ECpState.Finished);
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
        return _currentState == ECpState.Finished;
    }

    void SetState(ECpState newState)
    {
        if (_currentState == newState)
        {
            return;
        }

        _currentState = newState;

        switch (newState)
        {
            case ECpState.Update:
                // �X�V�����J�n���ɃX�e�[�g�J�n�������Ă�
                OnStart();
                break;
            case ECpState.Finished:
                OnFinished();
                break;
        }
    }
    public CpGamePlayManager OwnerGamePlayManager => _ownerGamePlayManager;
    CpGamePlayManager _ownerGamePlayManager = null;
    ECpState _currentState = ECpState.None;
}

// �������Ȃ��X�e�[�g
public class CpGamePlayStateRoot : CpGamePlayStateBase
{
    public override ECpGamePlayState GetGamePlayState()
    {
        return ECpGamePlayState.Root;
    }
    public override List<ECpGamePlayState> GetCanStackStates()
    {
        return null;
    }
}