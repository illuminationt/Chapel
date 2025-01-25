using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpDropItemParamScriptableObject")]
public class CpItemDropParamScriptableObject : ScriptableObject
{
#if CP_EDITOR
    [TextArea]
    public string Comment = null;
#endif

    public CpItemDropParam Param => _itemDropParam;
    [SerializeField]
    CpItemDropParam _itemDropParam;
}
