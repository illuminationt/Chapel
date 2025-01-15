using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;

public enum ECpShotSpawnLocationType
{
    None,
    Offset,
}

public struct FCpShotSpawnLocationRequestParam
{
    public Vector2 origin;
    public float forwardDegree;
}

[System.Serializable]
public struct FCpShotSpawnLocationParam_Offset
{
    public Vector2 GetLocation(in FCpShotSpawnLocationRequestParam reqParam)
    {
        Vector2 rotatedOffset = SltMath.RotateVector(Offset, reqParam.forwardDegree);
        return SltMath.AddVec(reqParam.origin, rotatedOffset);
    }
    [SerializeField] Vector2 Offset;
}

[System.Serializable]
public struct FCpShotSpawnLocationParam
{
    public Vector2 GetLocation(in FCpShotSpawnLocationRequestParam reqParam)
    {
        return LocationType switch
        {
            ECpShotSpawnLocationType.Offset => Param_Offset.GetLocation(reqParam),
            _ => throw new System.InvalidOperationException()
        };
    }
    [SerializeField] ECpShotSpawnLocationType LocationType;

    [SerializeField]
    [ShowIf("@LocationType==ECpShotSpawnLocationType.Offset")]
    FCpShotSpawnLocationParam_Offset Param_Offset;
}
