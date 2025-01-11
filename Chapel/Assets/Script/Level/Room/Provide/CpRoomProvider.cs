using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// �_���W�����̕��������p�����[�^
[System.Serializable]
public class CpRoomRequestParam
{
    public CpRoomRequestParam()
    {
        ConnectionFlag.Fill(true);
    }
    public bool IsValid()
    {
        if (!ConnectionFlag.Any())
        {
            return false;
        }
        return true;
    }

    public ECpRoomUsableType RoomUsableType;

    // ������k�A���t���O
    public TSltBitFlag<ECpRoomConnectDirectionType> ConnectionFlag;
}

//// 
//public static class CpRoomProvider
//{
//    public static CpRoom GetRoomPrefab(CpRoomRequestParam requestParam)
//    {
//        CpRoomProvideParamPerFloor paramPerDungeon = null;
//        List<CpRoom> roomPrefabs = paramPerDungeon.GetRoomPrefabList(requestParam.RoomUsableType, requestParam.ConnectionFlag);

//        return roomPrefabs[0];
//    }
//}
