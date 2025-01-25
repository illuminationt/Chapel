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
    public CpRoomProxyManager RoomProxyManager => _roomProxyManager;

    public void InitializeDungeon(CpFloorMasterDataScriptableObject floorMasterSettings)
    {
        _roomProxyManager.Initialize(floorMasterSettings);

        _roomProvideParam = floorMasterSettings.RoomProvideParamPerFloor;
        _floorStructureParam = floorMasterSettings.FloorStructureParam;
    }

    public void LandPlayer()
    {
        Vector2Int startPointIndex = _floorStructureParam.FindRoomIndex(ECpRoomUsableType.StartPoint);

        CpRoomProxy startRoomProxy = _roomProxyManager.FindRoomProxy(startPointIndex);

        startRoomProxy.OnLandPlayer();
        ActiveRoomProxy.CreateRoomInstance();

#if CP_DEBUG
        if (CpDebugParam.bEnableEnemyTest)
        {
            CpRoom roomInstance = ActiveRoomProxy.GetRoomInstance();
            roomInstance.OpenGate(false);
        }
#endif
        //_activeRoomProxy = CreateRoom(startPointIndex);
        //if (bOpen)
        //{
        //    _activeRoomProxy.MarkAsOpen();
        //}
        ////_activeRoomProxy.OnEnterRoom();
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
        roomTransitionParam.PrevRoomProxy = ActiveRoomProxy;
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
        if (proxy.GetRoomFlag(ECpRoomFlags.IsPlayerIn))
        {
            // 既に部屋に入ってたらスキップ
            return;
        }
        Assert.IsTrue(ActiveRoomProxy.Equals(proxy));
        proxy.OnEnterRoom();
    }

    public void ExitRoom(CpRoomProxyId roomProxyId)
    {
        CpRoomProxy proxy = _roomProxyManager.FindRoomProxy(roomProxyId);
        Assert.IsTrue(proxy.GetRoomFlag(ECpRoomFlags.IsPlayerIn));
        Assert.IsTrue(proxy == ActiveRoomProxy);
        proxy.OnExitRoom();
    }

    Vector2Int GetGoalRoomIndex(ECpRoomConnectDirectionType transitionDir)
    {
        Vector2Int retIndex = ActiveRoomProxy.GetRoomIndex();
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
    CpRoomProxy ActiveRoomProxy => _roomProxyManager.GetActiveRoomProxy();

    // 現在使用中のダンジョン構造パラメータ
    CpRoomProvideParamPerFloor _roomProvideParam = null;
    CpFloorStructureParam _floorStructureParam = null;

#if CP_DEBUG

    public static void CreateDummyRoom()
    {
        CpFloorMasterDataScriptableObject dummyFloorSettings = (CpFloorMasterDataScriptableObject)Resources.Load("DummyFloorMasterSettings");
        CpGamePlayManager gamePlayManager = CpGamePlayManager.Get();
        gamePlayManager.RequestEnterFloor(dummyFloorSettings);

        CpDungeonManager dungeonManager = CpDungeonManager.Get();
        //   dungeonManager.LandPlayer(false);
    }

    public void DrawImGui()
    {
        CpRoomProxyManager roomProxyManager = CpRoomProxyManager.Get();

        if (ImGui.CollapsingHeader("Room Proxy Manager"))
        {
            roomProxyManager.DrawImGui();
        }

        if (ImGui.CollapsingHeader("Current Room"))
        {
            if (ActiveRoomProxy != null)
            {
                ActiveRoomProxy.DrawImGui();
            }
            else
            {
                ImGui.Text("有効なRoomProxyが存在しません");
            }
        }
    }

#endif
}
