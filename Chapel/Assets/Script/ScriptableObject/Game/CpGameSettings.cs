using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpGameSettings")]
public class CpGameSettings : ScriptableObject
{
#if UNITY_EDITOR
    [TextArea]
    public string Comment = null;
#endif
    public static CpGameSettings Get() => CpGameManager.Instance.GameSettings;

    public CpAttackSenderParamScriptableObject AttackSenderParam;
}
