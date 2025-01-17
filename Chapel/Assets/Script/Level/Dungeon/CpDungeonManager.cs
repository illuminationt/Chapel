using ImGuiNET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class CpDungeonManager
{
    // static
    public static CpDungeonManager Create()
    {
        CpDungeonManager manager = new CpDungeonManager();
        manager._roomProxyManager = new CpRoomProxyManager();
        return manager;
    }

    public static CpDungeonManager Get() => CpGameManager.Instance.DungeonManager;

    public void InitializeDungeon(CpFloorMasterDataScriptableObject floorMasterSettings)
    {
        _roomProxyManager.Initialize(floorMasterSettings);

        _roomProvideParam = floorMasterSettings.RoomProvideParamPerFloor;
        _floorStructureParam = floorMasterSettings.FloorStructureParam;
    }

    public void LandPlayer()
    {
        Vector2Int startPointIndex = _floorStructureParam.FindRoomIndex(ECpRoomUsableType.StartPoint);

        _activeRoomProxy = _roomProxyManager.FindRoomProxy(startPointIndex);
        _activeRoomProxy.MarkAsOpen();

        _activeRoomProxy.CreateRoomInstance();
    }

    // 部屋遷移をリクエスト
    public void RequestRoomTransition(CpRoomTransitionRequestParam roomTransitionReqParam)
    {
        ECpRoomConnectDirectionType directionType = roomTransitionReqParam.ConnectDirection;
        Vector2Int goalRoomIndex = GetGoalRoomIndex(directionType);
        CpFloorStructureRoomParam roomParam = _floorStructureParam.FindRoomParam(goalRoomIndex.x, goalRoomIndex.y);

        if (roomParam == null)
        {
            Assert.IsTrue(false, "aa");
            return;
        }

        CpGamePlayManager gamePlayManager = CpGamePlayManager.Get();
        CpGamePlayStateRoomTransition stateRoomTransition = gamePlayManager.RequestStartState<CpGamePlayStateRoomTransition>(ECpGamePlayState.RoomTransition);

        CpRoomTransitionParam roomTransitionParam = new CpRoomTransitionParam();
        roomTransitionParam.DungeonManager = this;
        roomTransitionParam.PrevRoomProxy = _activeRoomProxy;
        roomTransitionParam.TransitionDirection = roomTransitionReqParam.ConnectDirection;
        stateRoomTransition.Setup(roomTransitionParam);
        stateRoomTransition.ReadyForActivation();
    }

    public CpRoomProxy CreateRoom(in Vector2Int roomIndex)
    {
        CpRoomProxy roomProxy = _roomProxyManager.FindRoomProxy(roomIndex);
        roomProxy.CreateRoomInstance();
        return roomProxy;
    }

    public void EnterRoom(CpRoomProxyId roomProxyId)
    {
        CpRoomProxy proxy = _roomProxyManager.FindRoomProxy(roomProxyId);
        Assert.IsTrue(_activeRoomProxy == null);
        _activeRoomProxy = proxy;
        _activeRoomProxy.OnEnterRoom();
    }
    public void ExitRoom(CpRoomProxyId roomProxyId)
    {
        CpRoomProxy proxy = _roomProxyManager.FindRoomProxy(roomProxyId);
        Assert.IsTrue(proxy == _activeRoomProxy);
        proxy.OnExitRoom();

        _activeRoomProxy = null;
    }


    Vector2Int GetGoalRoomIndex(ECpRoomConnectDirectionType transitionDir)
    {
        Vector2Int retIndex = _activeRoomProxy.GetRoomIndex();
        switch (transitionDir)
        {
            case ECpRoomConnectDirectionType.Up: retIndex.y--; break;
            case ECpRoomConnectDirectionType.Down: retIndex.y++; break;
            case ECpRoomConnectDirectionType.Left: retIndex.x--; break;
            case ECpRoomConnectDirectionType.Right: retIndex.x++; break;
            default:
                throw new System.NotImplementedException();
        }
        return retIndex;
    }

    CpRoomProxyManager _roomProxyManager = null;
    CpRoomProxy _activeRoomProxy = null;

    // 現在使用中のダンジョン構造パラメータ
    CpRoomProvideParamPerFloor _roomProvideParam = null;
    CpFloorStructureParam _floorStructureParam = null;

#if DEBUG
    public CpRoomProxyManager RoomProxyManager => _roomProxyManager;

    public void DrawImGui()
    {
        CpRoomProxyManager roomProxyManager = CpRoomProxyManager.Get();

        if (ImGui.CollapsingHeader("Room Proxy Manager"))
        {
            roomProxyManager.DrawImGui();
        }

        if (ImGui.CollapsingHeader("Current Room"))
        {
            if (_activeRoomProxy != null)
            {
                _activeRoomProxy.DrawImGui();
            }
            else
            {
                ImGui.Text("有効なRoomProxyが存在しません");
            }
        }
    }

#endif
}
