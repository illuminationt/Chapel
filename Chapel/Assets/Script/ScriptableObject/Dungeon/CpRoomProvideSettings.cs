using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpRoomProvideSettings")]
public class CpRoomProvideSettings : ScriptableObject
{
#if CP_EDITOR
    [TextArea]
    public string Comment = null;
#endif

}
