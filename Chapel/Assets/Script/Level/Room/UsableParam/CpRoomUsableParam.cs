using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class CpRoomUsableParam
{
    public CpRoomUsableParamBattle ParamBattle;
    public CpRoomUsableParamShop ParamShop;
    public CpRoomUsableParamBoss ParamBoss;
}

public enum ECpEnemySpawnParamSelectType
{
    None = -1,
    FixedData = 0,
}

[System.Serializable]
public class CpRoomUsableParamBattle
{
    public CpEnemySpawnParam EnemySpawnParam
    {
        get
        {
            return _paramSelectType switch
            {
                ECpEnemySpawnParamSelectType.FixedData => _spawnParamScriptableObject.EnemySpawnParam,
                _ => throw new System.NotImplementedException(),
            };
        }
    }

    [SerializeField]
    ECpEnemySpawnParamSelectType _paramSelectType = ECpEnemySpawnParamSelectType.FixedData;

    [SerializeField]
    [ShowIf("_paramSelectType", ECpEnemySpawnParamSelectType.FixedData)]
    CpEnemySpawnParamScriptableObject _spawnParamScriptableObject = null;
}

[System.Serializable]
public class CpRoomUsableParamShop
{

}

[System.Serializable]
public class CpRoomUsableParamBoss
{

}