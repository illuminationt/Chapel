using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ダンジョン1階層ごとに持つパラメータ

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpRoomProvideMasterParamScriptableObject")]
public class CpRoomProvideParamPerFloorScriptableObject : ScriptableObject
{
#if UNITY_EDITOR
    [TextArea]
    public string Comment = null;
#endif

    public CpRoomProvideParamPerFloor Param => _param;

    [SerializeField]
    CpRoomProvideParamPerFloor _param;

#if UNITY_EDITOR


    [Button("Validate")]
    public void Validate()
    {
        _param.Validate();
    }
#endif
}
