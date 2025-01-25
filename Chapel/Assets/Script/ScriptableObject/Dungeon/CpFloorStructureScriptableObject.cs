using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * フロアの部屋の配置場所、部屋同士の繋がりのデータ
 */
[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpFloorStructureScriptableObject")]
public class CpFloorStructureScriptableObject : ScriptableObject
{
#if CP_EDITOR
    [TextArea]
    public string Comment = null;

#endif

    public CpFloorStructureParam Param => _floorStructureParam;

    // フロアの構造
    [SerializeField]
    [ReadOnly]
    CpFloorStructureParam _floorStructureParam;
}
