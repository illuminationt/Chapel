using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public enum ECpGamePlayState
{
    None = -1,

    Root = 0,

    EnterDungeon = 1000,
    RoomBattle = 200,
    RoomExplore = 300,
    RoomTransition = 4000,

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
        return CpGameManager.Instance.GamePlayManager;
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

        _currentState = CpGamePlayStateUtil.CreateState<T>(newGamePlayState);
        return (T)_currentState;
    }

    CpGamePlayStateBase _currentState = null;
}
