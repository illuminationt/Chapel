using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CpMoveParamScriptableObjectBase : ScriptableObject
{
    public abstract CpMoveParamBase GetMoveParam();
}

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpMoveParamLinear")]
public class CpMoveParamLinearScriptableObject : CpMoveParamScriptableObjectBase
{
    public override CpMoveParamBase GetMoveParam()
    {
        return MoveParamLinear;
    }
    public CpMoveParamLinear MoveParamLinear;
}
