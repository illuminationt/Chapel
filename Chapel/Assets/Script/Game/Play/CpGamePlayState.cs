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
    // 明示的に開始
    public void ReadyForActivation()
    {
        SetState(ECpState.Update);
    }

    public void Update()
    {
        Assert.IsTrue(_currentState != ECpState.None, "ReadyForActivation()を呼んでないかも");
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

    // ステートの外からステートを終了させる
    public void ExternalCancel()
    {
        FinishState();
    }

    void OnStart()
    {
        OnStartInternal();
    }
    protected virtual void OnStartInternal() { }

    // 次のステートを指定せずにステートを終了させる
    protected void FinishState()
    {
        SetState(ECpState.Finished);
    }

    // 次のステートを指定してステートを終了させる
    // ステートのインスタンスをnewする関数を指定してください
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
                // 更新処理開始時にステート開始処理を呼ぶ
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

// 何もしないステート
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