using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CpMoveParamLinear : CpMoveParamBase
{
    public FCpMoveParamLinear ParamLinear;
}

[System.Serializable]
public struct FCpMoveParamLinear : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.Linear;

    public float Speed;
    public float MaxSpeedf;
    public float DegreeOffset;
    public float Accel;
}