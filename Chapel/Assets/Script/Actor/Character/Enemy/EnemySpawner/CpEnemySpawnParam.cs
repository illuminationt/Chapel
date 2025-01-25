using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using JetBrains.Annotations;
using UnityEngine.Assertions;

public enum ECpEnemySpawnLocationType
{
    None = -1,
    ScreenPosition = 0,
    Locator = 100,
}

[System.Serializable]
public class CpEnemySpawnLocationParam
{
    public Vector2 GetSpawnLocation()
    {
        return LocationType switch
        {
            ECpEnemySpawnLocationType.ScreenPosition => GetSpawnLocation_ScreenPosition(),
            _ => throw new System.NotImplementedException(),
        };
    }

    Vector2 GetSpawnLocation_ScreenPosition()
    {
        return CpUtil.GetWorldPositionFromNormalizedPosition(ScreenPosition);
    }

    Vector2 GetSpawnLocation_Locator()
    {
        CpLocator[] locators = Object.FindObjectsByType<CpLocator>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        for (int index = 0; index < locators.Length; index++)
        {
            bool bConditionMatch = locators[index].IsLocateConditionMatch(_locatorParam);
            if (bConditionMatch)
            {
                return locators[index].GetLocation();
            }
        }

        Assert.IsTrue(false);
        return Vector2.zero;
    }

    [SerializeField]
    ECpEnemySpawnLocationType LocationType;

    [SerializeField]
    [ShowIf("LocationType", ECpEnemySpawnLocationType.ScreenPosition)]
    Vector2 ScreenPosition;

    [SerializeField]
    [ShowIf("LocationType", ECpEnemySpawnLocationType.Locator)]
    CpLocatorParam _locatorParam = null;
}

[System.Serializable]
public class CpSpawnedEnemySpecificParam
{
    // スポーンからAI実行開始までのディレイ
    public float StartDelay = 0f;

    [SerializeReference]
    public List<CpEnemySpecificBehaviorBase> SpecificBehaviorList;
}

[System.Serializable]
public class CpEnemySpawnParamElement
{
    public bool bEnable = true;
    public CpEnemyBase Prefab;

    public CpEnemySpawnLocationParam LocationParam;
    public CpSpawnedEnemySpecificParam SpecificParam;
}

// 1つの部屋における敵キャラスポーンの情報すべてをまとめるクラス
[System.Serializable]
public class CpEnemySpawnParam
{
    public List<CpEnemySpawnParamElement> Elements = new List<CpEnemySpawnParamElement>();
}