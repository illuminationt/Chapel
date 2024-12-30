using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpHellParam")]
public class CpHellParamScriptableObject : ScriptableObject
{
#if UNITY_EDITOR
    [TextArea]
    public string Comment = null;
#endif
    public CpMultiHellParam MultiHellParam;
}
