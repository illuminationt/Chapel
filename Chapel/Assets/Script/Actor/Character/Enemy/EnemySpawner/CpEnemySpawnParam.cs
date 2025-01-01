using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using JetBrains.Annotations;

public enum ECpEnemySpawnLocationType
{
    ScreenPosition,
}

[System.Serializable]
public class CpEnemySpawnLocationParam
{
    public Vector2 GetSpawnLocation()
    {
        return LocationType switch
        {
            ECpEnemySpawnLocationType.ScreenPosition => GetSpawnLocationScreenPosition(),
            _ => throw new System.NotImplementedException(),
        };
    }

    Vector2 GetSpawnLocationScreenPosition()
    {
        return CpUtil.GetWorldPositionFromScreenPosition(ScreenPosition);
    }

    [SerializeField]
    ECpEnemySpawnLocationType LocationType;

    [SerializeField]
    [ShowIf("LocationType", ECpEnemySpawnLocationType.ScreenPosition)]
    Vector2 ScreenPosition;
}

[System.Serializable]
public class CpSpawnedEnemySpecificParam
{
    // スポーンからAI実行開始までのディレイ
    public float StartDelay = 0f;
}

[System.Serializable]
public class CpEnemySpawnParamElement
{
    public CpEnemyBase Prefab;

    public CpEnemySpawnLocationParam LocationParam;
    public CpSpawnedEnemySpecificParam SpecificParam;
}

[System.Serializable]
public class CpEnemySpawnParam
{
    public List<CpEnemySpawnParamElement> Elements = new List<CpEnemySpawnParamElement>();
}