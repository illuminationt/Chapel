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

    EnterDungeon = 1000, //�_���W�����ɓ��鉉�o
    RoomBattle = 200,// �����ł̐퓬��
    RoomExplore = 300,// �����ł̒T�����i�퓬���ȊO�j
    RoomTransition = 4000,//�����̊Ԃ̑@�ے�

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

// �_���W�����ł̃Q�[���v���C���̃Q�[���̗�����Ǘ�
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
            // ���݂Ȃ�̃X�e�[�g�������Ă��Ȃ�������\
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

        // ���ݎ��s���̃X�e�[�g���I��点��
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
