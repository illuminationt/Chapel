using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * １つのダンジョン（複数のフロアを連結したもの）のデータをすべてまとめて設定できる
 * ScriptableObject
 */
[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpDungeonMasterDataScriptableObject")]
public class CpDungeonMasterDataScriptableObject : ScriptableObject
{
#if CP_EDITOR
    [TextArea]
    public string Comment = null;
#endif

    public CpFloorMasterDataScriptableObject GetFloorSettings(int floorIndex)
    {
        return _floorSettings[floorIndex];
    }

    public int GetFloorNum() { return _floorSettings.Count; }

    [SerializeField]
    List<CpFloorMasterDataScriptableObject> _floorSettings = new List<CpFloorMasterDataScriptableObject>();
}
