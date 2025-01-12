using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpPrefabSettings")]
public class CpPrefabSettings : ScriptableObject
{
#if UNITY_EDITOR
    [TextArea]
    public string Comment = null;
#endif

    public static CpPrefabSettings Get()
    {
        return CpGameSettings.Get().PrefabSettings;
    }

    public CpItemDropper ItemDropperPrefab = null;
}
