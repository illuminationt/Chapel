using UnityEngine;
using UnityEngine.Assertions;
using ImGuiNET;

public enum ECpRoomFlags
{
    IsPlayerIn,//自機が存在する
    GateOpens,// ゲート解放済み
    BattleFinished,// エネミー討伐済み
}

public class CpRoomProxyId
{
    public static CpRoomProxyId Create()
    {
        CpRoomProxyId newId = new CpRoomProxyId();
        return newId;
    }
    public bool Equals(CpRoomProxyId otherId)
    {
        return _id.Equals(otherId._id);
    }
    CpRoomProxyId()
    {
        _id = FCpInstanceId.Create();
    }

    FCpInstanceId _id;
}

public class CpRoomProxy
{
    public static CpRoomProxy Create(CpFloorStructureRoomParam roomParam, CpRoomProvideParamPerFloor roomProvideParam, in Vector2Int roomIndex)
    {
        ECpRoomSelectType roomSelectType = roomParam.RoomSelectType;
        if (roomSelectType == ECpRoomSelectType.NoRoom)
        {
            // 部屋が存在しないなら部屋プロキシは作らない
            return null;
        }

        CpRoomProxy newProxy = new CpRoomProxy();
        CpRoom roomPrefab = roomParam.GetRoomPrefab(roomProvideParam);
        newProxy._roomPrefab = roomPrefab;
        newProxy._roomType = CpRoomUtil.ToRoomType(roomParam.GetRoomUsableType());
        newProxy._roomParam = roomParam;
        newProxy._roomProvideParam = roomProvideParam;

        // UsableParam設定
        newProxy._roomUsableParam = roomParam.GetOverrideRoomUsableParam() ?? roomPrefab.GetRoomUsableParam();

        newProxy._roomIndex = roomIndex;
        newProxy._id = CpRoomProxyId.Create();

        Assert.IsTrue(newProxy.IsAllParamValid());

        return newProxy;
    }

    public CpRoom CreateRoomInstance()
    {
        Assert.IsNotNull(_roomParam);

        if (_roomInstance != null)
        {
            _roomInstance.gameObject.SetActive(true);
            return _roomInstance;
        }

        CpRoom roomPrefab = _roomParam.GetRoomPrefab(_roomProvideParam);
        _roomInstance = CpRoom.CreateRoomInstance(roomPrefab, this);
        return _roomInstance;
    }
    public CpRoomProxyId GetId()
    {
        return _id;
    }
    public bool IsMatchId(CpRoomProxyId id)
    {
        if (_id == null || id == null)
        {
            Assert.IsTrue(false);
            return false;
        }
        return _id.Equals(id);
    }
    public void OnEnterRoom()
    {
        _roomInstance.OnEnterRoom();

        _roomFlags.Set(ECpRoomFlags.IsPlayerIn, true);
        TryStartGamePlayStateOnEnterRoom();
    }

    public void OnExitRoom()
    {
        _roomInstance.OnExitRoom();

        _roomFlags.Set(ECpRoomFlags.BattleFinished, true);
        _roomFlags.Set(ECpRoomFlags.IsPlayerIn, false);
    }

    public void OnAllRoomEnemyDestroyed()
    {
        CpEnemyShotEraser.Get().RequestErase();
        OpenGate(true);
    }

    public void OnRoomTransitionFinishedFromSelf()
    {
        Deactivate();
    }
    void OpenGate(bool bOpen)
    {
        _roomFlags.Set(ECpRoomFlags.GateOpens, bOpen);
        _roomInstance.OpenGate(bOpen);
    }

    //
    void TryStartGamePlayStateOnEnterRoom()
    {
        ECpRoomType roomType = GetRoomType();
        switch (roomType)
        {
            case ECpRoomType.None:
            case ECpRoomType.StartPoint:
                break;

            case ECpRoomType.Battle:
                TryStartGamePlayState_Battle();
                break;
            case ECpRoomType.Shop:
                break;
            case ECpRoomType.Boss:
                break;
        }
    }
    void TryStartGamePlayState_Battle()
    {
        if (_roomFlags.Get(ECpRoomFlags.BattleFinished))
        {
            // 既にこの部屋での戦闘終了済みなら何もしない
        }
        else
        {
            CpGamePlayManager gamePlayManager = CpGamePlayManager.Get();
            CpGamePlayStateRoomBattle stateBattle = gamePlayManager.RequestStartState<CpGamePlayStateRoomBattle>(ECpGamePlayState.RoomBattle);

            // todo RoomUsableParam
            stateBattle.Setup(this, GetRoomUsableParam().ParamBattle);
            stateBattle.ReadyForActivation();
        }
    }

    void Deactivate()
    {
        _roomInstance.gameObject.SetActive(false);
    }

    bool IsAllParamValid()
    {
        if (GetRoomType() == ECpRoomType.None) { return false; }
        if (GetRoomUsableParam() == null) { return false; }
        if (_roomIndex == new Vector2Int(-1, -1)) { return false; }
        return true;
    }
    //

    public ECpRoomType GetRoomType() => _roomType;
    public CpRoomUsableParam GetRoomUsableParam() => _roomUsableParam;
    public Vector2Int GetRoomIndex() => _roomIndex;
    public CpRoom GetRoomInstance() => _roomInstance;
    public bool GetRoomFlag(ECpRoomFlags flags) => _roomFlags.Get(flags);

    public void OnLandPlayer()
    {
        _roomFlags.Set(ECpRoomFlags.GateOpens, true);
        _roomFlags.Set(ECpRoomFlags.IsPlayerIn, true);
    }

    CpRoom _roomInstance = null;

    Vector2Int _roomIndex = new Vector2Int(-1, -1);
    CpRoom _roomPrefab = null;
    ECpRoomType _roomType = ECpRoomType.None;
    CpFloorStructureRoomParam _roomParam = null;
    CpRoomProvideParamPerFloor _roomProvideParam = null;
    CpRoomUsableParam _roomUsableParam = null;
    TSltBitFlag<ECpRoomFlags> _roomFlags;
    CpRoomProxyId _id = null;
#if CP_DEBUG
    public string GetRoomIndexString()
    {
        return $"{_roomIndex.x},{_roomIndex.y}";
    }

    public void DrawImGui()
    {
        if (ImGui.TreeNode("Room Instance"))
        {
            if (_roomInstance != null)
            {
                _roomInstance.DrawImGui();
            }
            else
            {
                ImGui.Text("Room Instance NOT Exists");
            }

            ImGui.TreePop();
        }

        if (ImGui.TreeNode("Room Flags"))
        {
            _roomFlags.DrawImGui("");
            ImGui.TreePop();
        }

        if (ImGui.TreeNode("CallFunc for Debug"))
        {
            ImGui.TreePop();
        }
    }
#endif
}
