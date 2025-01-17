using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CpRoomTransitionParam
{
    public bool IsValidParam()
    {
        return DungeonManager != null && PrevRoomProxy != null && TransitionDirection != ECpRoomConnectDirectionType.None;
    }
    public CpDungeonManager DungeonManager = null;
    public CpRoomProxy PrevRoomProxy = null;
    public ECpRoomConnectDirectionType TransitionDirection = ECpRoomConnectDirectionType.None;
}

public class CpGamePlayStateRoomTransition : CpGamePlayStateBase
{
    enum ECpState
    {
        None,
        TranslateRooms,// �����C���X�^���X�𕽍s�ړ�������
        ShowNewRoom,// �V���������̕\��

        Finished,
    }
    public override ECpGamePlayState GetGamePlayState()
    {
        return ECpGamePlayState.RoomTransition;
    }
    public override List<ECpGamePlayState> GetCanStackStates()
    {
        return new List<ECpGamePlayState> {
            ECpGamePlayState.RoomExplore,};
    }

    public void Setup(CpRoomTransitionParam param)
    {
        _param = param;
        _prevRoomInstance = _param.PrevRoomProxy.GetRoomInstance();

        Assert.IsTrue(_param.IsValidParam());
    }

    protected override void OnStartInternal()
    {
        SetPlayable(false);

        // �ړ���̕������쐬
        Vector2Int prevRoomIndex = _param.PrevRoomProxy.GetRoomIndex();
        Vector2Int nextRoomIndex = CpRoomUtil.GetNextRoomIndex(prevRoomIndex, _param.TransitionDirection);
        _nextRoomProxy = _param.DungeonManager.CreateRoom(nextRoomIndex);
        _nextRoomInstance = _nextRoomProxy.GetRoomInstance();
        _nextRoomInstance.gameObject.SetActive(false);
        _nextRoomInstance.transform.position = Vector3.zero;


        // �����J�ڎ��̈ړ��ʂ��Z�o
        // �ړ��O�̕����̃Q�[�g->�ړ���̕����̃Q�[�g
        //Vector2 roomTranslation = prevRoomGatePosition - nextRoomGatePosition;
        CpRoom prevRoom = _param.PrevRoomProxy.GetRoomInstance();
        _prevRoomInitPosition = prevRoom.transform.position;
        _prevRoomGoalPosition = _prevRoomInitPosition + prevRoom.GetTranslationOnTransition(_nextRoomInstance, _param.TransitionDirection);

        // ���@���ړ���̕����̃Q�[�g������Ɉړ�������
        CpPlayer player = CpPlayer.Get();
        _playerInirPosition = player.transform.position;
        _playerGoalPosition = prevRoom.GetPlayerPositionOnEnterRoom(_nextRoomInstance, _param.TransitionDirection);

        // �J�ڊ֘A�̃R�[���o�b�N
        CpDungeonManager dungeonManager = CpDungeonManager.Get();
        dungeonManager.ExitRoom(_param.PrevRoomProxy.GetId());

        SetState(ECpState.TranslateRooms);
    }

    protected override void UpdateInternal()
    {
        switch (_currentState)
        {
            case ECpState.TranslateRooms:
                UpdateTranslateRooms();
                break;
            case ECpState.ShowNewRoom:
                UpdateShowNewRooms();
                break;
            case ECpState.Finished:
                UpdateFinished();
                break;
            default:
                Assert.IsTrue(false);
                break;
        }
    }

    protected override void OnFinishedInternal()
    {
        // �ی�����
        SetPlayable(true);

        // �J�ڊ֘A�̃R�[���o�b�N
        CpDungeonManager dungeonManager = CpDungeonManager.Get();
        dungeonManager.EnterRoom(_nextRoomProxy.GetId());
        CpDebug.Log($"Room Transition {_param.PrevRoomProxy.GetRoomIndexString()}->{_nextRoomProxy.GetRoomIndexString()}");
    }

    void SetState(ECpState newState)
    {
        if (_currentState == newState)
        {
            Assert.IsTrue(false);
            return;
        }

        _currentState = newState;
        switch (newState)
        {
            case ECpState.TranslateRooms:
                OnStartUpdateTranslateRooms();
                break;

            case ECpState.ShowNewRoom:
                OnStartShowNewRoom();
                break;

            case ECpState.Finished:
                OnStartFinished();
                break;

            default:
                Assert.IsTrue(false);
                break;
        }
    }

    void UpdateTranslateRooms()
    {
        _tweenAlpha += CpTime.DeltaTime / _transitionDuration;

        CpPlayer player = CpPlayer.Get();

        if (_tweenAlpha > 1f)
        {
            _tweenAlpha = 1f;
            _prevRoomInstance.transform.position = _prevRoomGoalPosition;
            player.transform.position = _playerGoalPosition;
            SetState(ECpState.ShowNewRoom);
        }

        Vector2 prevRoomPosition = SltMath.Lerp(_prevRoomInitPosition, _prevRoomGoalPosition, _tweenAlpha);
        _prevRoomInstance.transform.position = prevRoomPosition;

        Vector2 playerPosition = SltMath.Lerp(_playerInirPosition, _playerGoalPosition, _tweenAlpha);
        player.transform.position = playerPosition;
    }

    void OnStartUpdateTranslateRooms()
    {

    }

    void UpdateShowNewRooms()
    {
        _showNewRoomTimer += CpTime.DeltaTime;
        if (_showNewRoomTimer > 0f)
        {
            SetState(ECpState.Finished);
        }
    }
    void OnStartShowNewRoom()
    {
        _nextRoomInstance.gameObject.SetActive(true);
    }

    void UpdateFinished()
    {

    }
    void OnStartFinished()
    {
        SetPlayable(true);

        _nextRoomProxy.OnEnterRoom();
        _param.PrevRoomProxy.OnRoomTransitionFinishedFromSelf();
        FinishState();
    }

    void SetPlayable(bool bPlayable)
    {
        CpPlayer player = CpPlayer.Get();

        bool bCollisionEnabled = bPlayable;
        player.SetCollisionEnabled(bCollisionEnabled);
    }

    ECpState _currentState = ECpState.None;

    // ��ŊO�������邩��
    float _transitionDuration = 0.4f;

    // ����
    CpRoomTransitionParam _param = null;
    CpRoom _prevRoomInstance = null;
    CpRoomProxy _nextRoomProxy = null;
    CpRoom _nextRoomInstance = null;

    // ���ʂŎg��Tweener
    float _tweenAlpha = 0f;


    // Transition�֘A
    // �ړ��O�̕����֘A
    Vector2 _prevRoomInitPosition = Vector2.zero;
    Vector2 _prevRoomGoalPosition = Vector2.zero;


    // ���@�ړ��֘A
    Vector2 _playerInirPosition = Vector2.zero;
    Vector2 _playerGoalPosition = Vector2.zero;

    // ShowNewRoom�֘A
    float _showNewRoomTimer = 0f;
}
