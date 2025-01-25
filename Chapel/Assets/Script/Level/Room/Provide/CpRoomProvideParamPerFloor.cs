using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;

#if CP_EDITOR
using UnityEditor;
#endif

// 同じ役割を持つ部屋のリスト
// ex:上下方向につながった戦闘部屋
[System.Serializable]
public class CpRoomProvideParamSameUse
{
    public List<CpRoom> Rooms = new List<CpRoom>();

#if CP_EDITOR
    public void Reset()
    {
        Rooms.Clear();
    }
#endif
}

// 部屋の種類ごとの供給パラメータ
[System.Serializable]
public class CpRoomProvideParamPerRoomType
{
    public CpRoomProvideParamSameUse GetProvideParamSameUse(TSltBitFlag<ECpRoomConnectDirectionType> dirTypeFlag)
    {
        bool u = dirTypeFlag.Get(ECpRoomConnectDirectionType.Up);
        bool d = dirTypeFlag.Get(ECpRoomConnectDirectionType.Down);
        bool l = dirTypeFlag.Get(ECpRoomConnectDirectionType.Left);
        bool r = dirTypeFlag.Get(ECpRoomConnectDirectionType.Right);

        if (u & d & l & r) { return UDLR; }
        else if (d & l & r) { return DLR; }
        else if (u & l & r) { return ULR; }
        else if (u & d & r) { return UDR; }
        else if (u & d & l) { return UDL; }
        else if (l & r) { return LR; }
        else if (d & r) { return DR; }
        else if (d & l) { return DL; }
        else if (u & r) { return UR; }
        else if (u & l) { return UL; }
        else if (u & d) { return UD; }
        else if (r) { return R; }
        else if (l) { return L; }
        else if (d) { return D; }
        else if (u) { return U; }
        else { return null; }
    }

    public CpRoomProvideParamSameUse No;
    public CpRoomProvideParamSameUse U;
    public CpRoomProvideParamSameUse D;
    public CpRoomProvideParamSameUse L;
    public CpRoomProvideParamSameUse R;
    public CpRoomProvideParamSameUse UD;
    public CpRoomProvideParamSameUse UL;
    public CpRoomProvideParamSameUse UR;
    public CpRoomProvideParamSameUse DL;
    public CpRoomProvideParamSameUse DR;
    public CpRoomProvideParamSameUse LR;
    public CpRoomProvideParamSameUse UDL;
    public CpRoomProvideParamSameUse UDR;
    public CpRoomProvideParamSameUse ULR;
    public CpRoomProvideParamSameUse DLR;
    public CpRoomProvideParamSameUse UDLR;

#if CP_EDITOR
    public void SetRoomPrefabs(List<CpRoom> roomPrefabs)
    {
        VisitParams((CpRoomProvideParamSameUse paramSameUse, string paramName) =>
        {
            paramSameUse.Reset();
        });

        foreach (CpRoom roomPrefab in roomPrefabs)
        {
            TSltBitFlag<ECpRoomConnectDirectionType> roomConnectFlag = roomPrefab.RoomConnectFlag;
            CpRoomProvideParamSameUse paramSameUse = GetProvideParamSameUse(roomConnectFlag);
            paramSameUse.Rooms.Add(roomPrefab);
        }
    }
    public void Validate()
    {
        VisitParams((CpRoomProvideParamSameUse paramSameUse, string paramName) =>
        {
            CpDebug.Log("AA:" + nameof(paramSameUse));
        });
    }

    void VisitParams(UnityAction<CpRoomProvideParamSameUse, string> visitFunc)
    {
        visitFunc(No, nameof(No));
        visitFunc(U, nameof(U));
        visitFunc(D, nameof(D));
        visitFunc(L, nameof(L));
        visitFunc(R, nameof(R));
        visitFunc(UD, nameof(UD));
        visitFunc(UL, nameof(UL));
        visitFunc(UR, nameof(UR));
        visitFunc(DL, nameof(DL));
        visitFunc(DR, nameof(DR));
        visitFunc(LR, nameof(LR));
        visitFunc(UDL, nameof(UDL));
        visitFunc(UDR, nameof(UDR));
        visitFunc(ULR, nameof(ULR));
        visitFunc(DLR, nameof(DLR));
        visitFunc(UDLR, nameof(UDLR));
    }

#endif
}

// ダンジョン１階層毎の部屋供給パラメータ
[System.Serializable]
public class CpRoomProvideParamPerFloor
{
    public CpRoom FindRoomPrefab(CpRoomRequestParam roomRequestParam)
    {
        List<CpRoom> roomCandidates = GetRoomPrefabList(roomRequestParam.RoomUsableType, roomRequestParam.ConnectionFlag);
        CpRoom retRoomPrefab = roomCandidates.Random();
        return retRoomPrefab;
    }

    List<CpRoom> GetRoomPrefabList(ECpRoomUsableType roomUsableType, TSltBitFlag<ECpRoomConnectDirectionType> connectDirFlag)
    {
        CpRoomProvideParamPerRoomType paramPerRoomType = GetRoomProvideParamPerRoomType(roomUsableType);
        CpRoomProvideParamSameUse paramSameUse = paramPerRoomType.GetProvideParamSameUse(connectDirFlag);
        return paramSameUse.Rooms;
    }

    CpRoomProvideParamPerRoomType GetRoomProvideParamPerRoomType(ECpRoomUsableType roomUsableType)
    {
        return roomUsableType switch
        {
            ECpRoomUsableType.StartPoint => _startPoint,
            ECpRoomUsableType.Battle => _battle,
            ECpRoomUsableType.PlaceObject => _placeObject,
            ECpRoomUsableType.Shop => _shop,
            ECpRoomUsableType.Boss => _boss,
            _ => throw new System.NotImplementedException(),
        };
    }

    [SerializeField]
    [LabelText("使用可能な部屋プレハブIDリスト")]
    [Tooltip("「データ収集」は、このIDリストを用いてパラメータを自動設定する")]
    List<int> UsableIdList = new List<int>();

    // 以下、自動設定
    [SerializeField][ReadOnly] CpRoomProvideParamPerRoomType _startPoint;
    [SerializeField][ReadOnly] CpRoomProvideParamPerRoomType _battle;
    [SerializeField][ReadOnly] CpRoomProvideParamPerRoomType _placeObject;
    [SerializeField][ReadOnly] CpRoomProvideParamPerRoomType _shop;
    [SerializeField][ReadOnly] CpRoomProvideParamPerRoomType _boss;

#if CP_EDITOR
    public void Validate()
    {
        _startPoint.Validate();
    }

    [Button("データ収集")]
    void Collect()
    {
        _startPoint.SetRoomPrefabs(CollectRooms(UsableIdList, ECpRoomUsableType.StartPoint));
        _battle.SetRoomPrefabs(CollectRooms(UsableIdList, ECpRoomUsableType.Battle));
        _placeObject.SetRoomPrefabs(CollectRooms(UsableIdList, ECpRoomUsableType.PlaceObject));
        _shop.SetRoomPrefabs(CollectRooms(UsableIdList, ECpRoomUsableType.Shop));
        _boss.SetRoomPrefabs(CollectRooms(UsableIdList, ECpRoomUsableType.Boss));
    }

    List<CpRoom> CollectRooms(List<int> usableIdList, ECpRoomUsableType roomUsableType)
    {
        return Collect(usableIdList, roomUsableType).Select(go => go.GetComponent<CpRoom>()).ToList();
    }

    List<GameObject> Collect(List<int> usableIdList, ECpRoomUsableType roomUsableType)
    {
        string rootFolderPath = "Assets/Content/Level/Room";

        string[] guids = AssetDatabase.FindAssets("t:prefab", new[] { rootFolderPath });
        List<GameObject> matchingPrefabs = new List<GameObject>();

        foreach (string guid in guids)
        {
            // GUIDからアセットパスを取得
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            // プレハブをロード
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab == null)
            {
                continue;
            }

            CpRoom room = prefab.GetComponent<CpRoom>();
            if (room == null)
            {
                continue;
            }

            if (!usableIdList.Contains(room.UsableId))
            {// 使用可能ダンジョンIDが合致しないなら不可
                continue;
            }

            if (roomUsableType != room.RoomUsableType)
            {
                continue;
            }

            matchingPrefabs.Add(prefab);
        }

        return matchingPrefabs;
    }
#endif
}
