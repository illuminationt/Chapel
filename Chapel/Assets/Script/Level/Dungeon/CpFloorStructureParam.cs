using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Unity.VisualScripting;

public enum ECpRoomSelectType
{
    NoRoom,
    Fixed,// 固定のプレハブを使用
    FromProvider,// プロバイダから受け取る
}

[System.Serializable]
public class CpFixedRoomParam
{
    public CpRoom GetRoomPrefab() => RoomPrefab;

    [SerializeField]
    CpRoom RoomPrefab = null;
}

public enum ECpRoomConnectDirectionType
{
    None = -1,
    Up = 0,    // 北方向
    Down = 1,  //南方向
    Left = 2,  //東方向
    Right = 3,  // 西方向
}

public enum ECpRoomConnectAxisType
{
    None,
    Horizontal,
    Vertical,
}

// ダンジョンを構成する各部屋のパラメータ
[System.Serializable]
public class CpFloorStructureRoomParam
{
    public CpRoom GetRoomPrefab(CpRoomProvideParamPerFloor roomProvideParam)
    {
        return _roomSelectType switch
        {
            ECpRoomSelectType.NoRoom => null,
            ECpRoomSelectType.Fixed => _fixedRoomParam.GetRoomPrefab(),
            ECpRoomSelectType.FromProvider => roomProvideParam.FindRoomPrefab(_provideParam),
            _ => throw new System.NotImplementedException(),
        };
    }

    public ECpRoomUsableType GetRoomUsableType()
    {
        switch (_roomSelectType)
        {
            case ECpRoomSelectType.NoRoom:
                return ECpRoomUsableType.None;
            case ECpRoomSelectType.Fixed:
                return _fixedRoomParam.GetRoomPrefab().RoomUsableType;
            case ECpRoomSelectType.FromProvider:
                return _provideParam.RoomUsableType;
            default:
                throw new System.NotImplementedException();
        }
    }

    [SerializeField]
    ECpRoomSelectType _roomSelectType;

    [SerializeField]
    [ShowIf("_roomSelectType", ECpRoomSelectType.Fixed)]
    CpFixedRoomParam _fixedRoomParam = new CpFixedRoomParam();

    [SerializeField]
    [ShowIf("_roomSelectType", ECpRoomSelectType.FromProvider)]
    CpRoomRequestParam _provideParam = new CpRoomRequestParam();

#if UNITY_EDITOR
    public ECpRoomSelectType RoomSelectType { get { return _roomSelectType; } set { _roomSelectType = value; } }
    public CpFixedRoomParam FixedRoomParam { get { return _fixedRoomParam; } set { _fixedRoomParam = value; } }
    public CpRoomRequestParam ProvideParam { get { return _provideParam; } set { _provideParam = value; } }
#endif
}

[System.Serializable]
public class CpFloorStructureRow
{
    public List<CpFloorStructureRoomParam> RowParams = new List<CpFloorStructureRoomParam>();
}

// フロアを作成するのに使用するパラメータ
// CpDungeonStructureCreatorによって作成されるパラメータ。
// エディタで手動で作成することも可能

[System.Serializable]
public class CpFloorStructureParam
{
    public Vector2Int FindRoomIndex(ECpRoomUsableType roomType)
    {
        Vector2Int retRoomIndex = Vector2Int.zero;
        bool bFound = VisitRoomParam((CpFloorStructureRoomParam roomParam, Vector2Int index) =>
        {
            ECpRoomUsableType thisRoomType = roomParam.GetRoomUsableType();
            if (thisRoomType == roomType)
            {
                retRoomIndex = index;
                return true;
            }
            return false;
        });

        Assert.IsTrue(bFound);
        return retRoomIndex;
    }
    public CpFloorStructureRoomParam FindRoomParam(ECpRoomUsableType roomType)
    {
        Vector2Int index = FindRoomIndex(roomType);
        return FindRoomParam(index.x, index.y);
    }
    public CpFloorStructureRoomParam FindRoomParam(int x, int y)
    {
        if (!SltUtil.IsValidIndex(_roomParamList, y))
        {
            return null;
        }
        CpFloorStructureRow rowParams = _roomParamList[y];
        if (rowParams == null || !SltUtil.IsValidIndex(rowParams.RowParams, x))
        {
            return null;
        }
        CpFloorStructureRoomParam param = rowParams.RowParams[x];
        return param;
    }

    bool VisitRoomParam(Func<CpFloorStructureRoomParam, Vector2Int, bool> visitFunc)
    {
        for (int y = 0; y < RoomHeight; y++)
        {
            for (int x = 0; x < RoomWidth; x++)
            {
                CpFloorStructureRoomParam roomParam = FindRoomParam(x, y);
                bool bResult = visitFunc(roomParam, new Vector2Int(x, y));
                if (bResult)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public int RoomHeight => _roomHeight;
    public int RoomWidth => _roomWidth;

    List<CpFloorStructureRoomParam> GetRowParams(int row) => _roomParamList[row].RowParams;

    // 各部屋のパラメータ。
    // FindRoomParam経由でアクセスして下さい
    [SerializeField]
    List<CpFloorStructureRow> _roomParamList = null;
    // ダンジョンの行の数
    [SerializeField]
    int _roomHeight = 1;
    // ダンジョンの列の数
    [SerializeField]
    int _roomWidth = 1;

#if UNITY_EDITOR
    public void SetDungeonSize(int x, int y)
    {
        _roomWidth = x;
        _roomHeight = y;
        Validate(_roomWidth, _roomHeight);

        // まず高さを更新
        if (_roomParamList.Count < _roomHeight)
        {
            while (_roomParamList.Count < _roomHeight)
            {
                CpFloorStructureRow newRow = new CpFloorStructureRow();
                _roomParamList.Add(newRow);
            }
        }

        // 幅を更新
        for (int yindex = 0; yindex < _roomHeight; yindex++)
        {

            for (int xindex = 0; xindex < RoomWidth; xindex++)
            {
                List<CpFloorStructureRoomParam> roomParamList = GetRowParams(yindex);
                while (roomParamList.Count < _roomWidth)
                {
                    roomParamList.Add(new CpFloorStructureRoomParam());
                }
            }
        }
    }

    public void Translate(int deltaX, int deltaY)
    {
        SltUtil.ShiftListElement(ref _roomParamList, deltaY);
        foreach (var roomParam in _roomParamList)
        {
            if (roomParam == null)
            {
                continue;
            }
            SltUtil.ShiftListElement(ref roomParam.RowParams, deltaX);
        }
    }

    public void Validate(int width, int height)
    {
        // サイズを設定
        SltUtil.ResizeList(ref _roomParamList, height);

        // nullのところを埋める
        for (int y = 0; y < _roomParamList.Count; y++)
        {
            if (_roomParamList[y] == null)
            {
                _roomParamList[y] = new CpFloorStructureRow();
            }

            for (int x = 0; x < _roomParamList[y].RowParams.Count; x++)
            {
                if (_roomParamList[y].RowParams[x] == null)
                {
                    _roomParamList[y].RowParams[x] = new CpFloorStructureRoomParam();
                }
            }
        }
        foreach (var rowParams in _roomParamList)
        {
            SltUtil.ResizeList(ref rowParams.RowParams, width);
        }

    }


#endif
}
