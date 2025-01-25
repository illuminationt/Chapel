using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpMoveParamScriptableObject")]
public class CpMoveParamScriptableObject : ScriptableObject
{
    [SerializeReference]
    public ICpMoveParam MoveParam;
}
