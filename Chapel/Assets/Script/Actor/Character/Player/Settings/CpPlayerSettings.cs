using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpPlayerSettings")]
public class CpPlayerSettings : ScriptableObject
{
#if CP_EDITOR
    [TextArea]
    public string Comment = null;
#endif

    public static CpPlayerSettings Get() => CpGameSettings.Get().PlayerSettings;

    public CpPlayerAimMarkerParam AimMarkerParam;
}
