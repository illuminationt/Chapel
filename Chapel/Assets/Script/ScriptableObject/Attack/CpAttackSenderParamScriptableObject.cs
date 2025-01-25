using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpAttackableMatrixScriptableObject")]
public class CpAttackSenderParamScriptableObject : ScriptableObject
{
#if CP_EDITOR
    [TextArea]
    public string Comment = null;
#endif
    public static CpAttackSenderParamScriptableObject Get() => CpGameSettings.Get().AttackSenderParam;
    public CpAttackSenderParam AttackSenderParam;
}
