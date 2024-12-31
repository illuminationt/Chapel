using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CpMoveParamCurve : CpMoveParamBase
{
    public FCpMoveParamCurve ParamCurve;
}

[System.Serializable]
public struct FCpMoveParamCurve : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.Curve;

    public float Speed;
    public float MaxSpeed;
    public float Accel;
    public float Degree;
    public float AngularSpeed;
    public float MaxCurveDegree;
}