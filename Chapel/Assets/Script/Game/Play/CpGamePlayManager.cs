using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum ECpGamePlayState
{
    None = -1,

    Root = 0,

    EnterDungeon = 1000, //ダンジョンに入る演出
    RoomBattle = 200,// 部屋での戦闘中
    RoomExplore = 300,// 部屋での探索中（戦闘中以外）
    RoomTransition = 4000,//部屋の間の繊維中

    SceneTransition = 100000,
}

public class CpGamePlayStateStack
{
    public void Push(CpGamePlayStateBase state)
    {
        if (Contains(state)) { return; }

        _stack.Add(state);
    }

    public CpGamePlayStateBase Pop()
    {
        if (_stack.Count == 0)
        {
            return null;
        }

        CpGamePlayStateBase last = Peek();
        _stack.RemoveAt(_stack.Count - 1);
        return last;
    }

    public CpGamePlayStateBase Peek()
    {
        if (_stack.Count == 0)
        {
            return null;
        }

        return _stack.Last();
    }

    bool Contains(CpGamePlayStateBase inState)
    {
        ECpGamePlayState inStateType = inState.GetGamePlayState();
        for (int index = 0; index < _stack.Count; index++)
        {
            if (_stack[index].GetGamePlayState() == inStateType)
            {
                return true;
            }
        }
        return false;
    }

    List<CpGamePlayStateBase> _stack = new List<CpGamePlayStateBase>();
}

// ダンジョンでのゲームプレイ中のゲームの流れを管理
public class CpGamePlayManager
{
    public static CpGamePlayManager Create()
    {
        CpGamePlayManager instance = new CpGamePlayManager();
        instance.Initialize();
        return instance;
    }
    void Initialize()
    {
        CpGamePlayStateRoot stateRoot = RequestStartState<CpGamePlayStateRoot>(ECpGamePlayState.Root);
        stateRoot.ReadyForActivation();
    }

    public static CpGamePlayManager Get()
    {
        return CpGameManager.Instance?.GamePlayManager;
    }

    bool CanStartState(ECpGamePlayState newState)
    {
        if (_currentState == null)
        {
            // 現在なんのステートも走っていなかったら可能
            return true;
        }

        return true;
    }

    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.Update();
            if (_currentState.IsFinished())
            {
                _currentState = null;
            }
        }
    }

    public void RequestEnterFloor(CpFloorMasterDataScriptableObject floorMasterSettings)
    {
        CpGamePlayStateEnterFloor stateEnterFloor = RequestStartState<CpGamePlayStateEnterFloor>(ECpGamePlayState.EnterDungeon);
        stateEnterFloor.Setup(floorMasterSettings);
        stateEnterFloor.ReadyForActivation();
    }

    public void RequestStartRoomExplore()
    {
        CpGamePlayStateRoomExplore stateRoomExplore = RequestStartState<CpGamePlayStateRoomExplore>(ECpGamePlayState.RoomExplore);
        stateRoomExplore.ReadyForActivation();
    }

    public void RequestStartSceneTransition(ECpSceneType sceneType)
    {
    }
    public T RequestStartState<T>(ECpGamePlayState newGamePlayState) where T : CpGamePlayStateBase
    {
        if (!CanStartState(newGamePlayState))
        {
            return null;
        }

        // 現在実行中のステートを終わらせる
        if (_currentState != null)
        {
            _currentState.ExternalCancel();
            _currentState = null;
        }

        _currentState = CpGamePlayStateUtil.CreateState<T>(this, newGamePlayState);
        return (T)_currentState;
    }

    public void InvokeGamePlayStateSeqChangedEvent(ECpGamePlayState state, ECpGamePlayStateSeq seq)
    {
        OnGamePlayStateSeqUpdated.Invoke(state, seq);

        switch (state)
        {
            case ECpGamePlayState.EnterDungeon:
                if (seq == ECpGamePlayStateSeq.Start)
                {
                    OnStartPlayable.Invoke();
                }
                break;
        }
    }

    public UnityEvent<ECpGamePlayState, ECpGamePlayStateSeq> OnGamePlayStateSeqUpdated => _stateSeqEvent;
    public UnityEvent OnStartPlayable => _onStartPlayable;

    CpGamePlayStateBase _currentState = null;
    UnityEvent<ECpGamePlayState, ECpGamePlayStateSeq> _stateSeqEvent = new UnityEvent<ECpGamePlayState, ECpGamePlayStateSeq>();
    UnityEvent _onStartPlayable = new UnityEvent();
    UnityEvent _onEndPlayable = new UnityEvent();
#if CP_DEBUG

    public void DrawImGui()
    {
        _currentState.DrawImGui();
    }
#endif
}
