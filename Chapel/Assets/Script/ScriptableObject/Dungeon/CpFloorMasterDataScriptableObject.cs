using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1つのフロア（１階層）を構成するすべての要素をまとめたデータ
[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpFloorMasterDataScriptableObject")]
public class CpFloorMasterDataScriptableObject : ScriptableObject
{
#if UNITY_EDITOR
    [TextArea]
    public string Comment = null;

    [InfoBox("１つの階層がフロアを構築するのに必要なパラメータを全てまとめたScriptableObject.")]

#endif
    public CpRoomProvideParamPerFloor RoomProvideParamPerFloor => _roomProvideSettings.Param;
    // フロアの部屋プレハブ供給パラメータ
    [SerializeField]
    [LabelText("Roomの設定方法が「FromProvider」となっている部屋の供給源")]
    CpRoomProvideParamPerFloorScriptableObject _roomProvideSettings = null;

    public CpFloorStructureParam FloorStructureParam => _floorStructureSettings.Param;
    // フロアの構造
    [SerializeField]
    [LabelText("フロアの構造.具体的なRoomの配置場所,StartPointやBoss部屋の場所等.")]
    CpFloorStructureScriptableObject _floorStructureSettings = null;
}
