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
        TranslateRooms,// 部屋インスタンスを平行移動させる
        ShowNewRoom,// 新しい部屋の表示

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

        // 移動先の部屋を作成
        Vector2Int prevRoomIndex = _param.PrevRoomProxy.GetRoomIndex();
        Vector2Int nextRoomIndex = CpRoomUtil.GetNextRoomIndex(prevRoomIndex, _param.TransitionDirection);
        _nextRoomProxy = _param.DungeonManager.CreateRoom(nextRoomIndex);
        _nextRoomInstance = _nextRoomProxy.GetRoomInstance();
        _nextRoomInstance.gameObject.SetActive(false);
        _nextRoomInstance.transform.position = Vector3.zero;


        // 部屋遷移時の移動量を算出
        // 移動前の部屋のゲート->移動後の部屋のゲート
        //Vector2 roomTranslation = prevRoomGatePosition - nextRoomGatePosition;
        CpRoom prevRoom = _param.PrevRoomProxy.GetRoomInstance();
        _prevRoomInitPosition = prevRoom.transform.position;
        _prevRoomGoalPosition = _prevRoomInitPosition + prevRoom.GetTranslationOnTransition(_nextRoomInstance, _param.TransitionDirection);

        // 自機を移動後の部屋のゲート入り口に移動させる
        CpPlayer player = CpPlayer.Get();
        _playerInirPosition = player.transform.position;
        _playerGoalPosition = prevRoom.GetPlayerPositionOnEnterRoom(_nextRoomInstance, _param.TransitionDirection);

        // 遷移関連のコールバック
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
        // 保険処理
        SetPlayable(true);

        // 遷移関連のコールバック
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

    // 後で外部化するかも
    float _transitionDuration = 0.4f;

    // 共通
    CpRoomTransitionParam _param = null;
    CpRoom _prevRoomInstance = null;
    CpRoomProxy _nextRoomProxy = null;
    CpRoom _nextRoomInstance = null;

    // 共通で使うTweener
    float _tweenAlpha = 0f;


    // Transition関連
    // 移動前の部屋関連
    Vector2 _prevRoomInitPosition = Vector2.zero;
    Vector2 _prevRoomGoalPosition = Vector2.zero;


    // 自機移動関連
    Vector2 _playerInirPosition = Vector2.zero;
    Vector2 _playerGoalPosition = Vector2.zero;

    // ShowNewRoom関連
    float _showNewRoomTimer = 0f;
}
