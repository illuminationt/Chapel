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

#if CP_DEBUG
    public ECpEnemySpawnParamSelectType ParamSelectType { get => _paramSelectType; set { _paramSelectType = value; } }
    public CpEnemySpawnParamScriptableObject SpawnParamScriptableObject { get => _spawnParamScriptableObject; set { _spawnParamScriptableObject = value; } }
#endif
}

[System.Serializable]
public class CpRoomUsableParamShop
{

}

[System.Serializable]
public class CpRoomUsableParamBoss
{

}

// Roomに設定されたUsableパラメータをオーバーライドするか否か決めるパラメータ
// オーバーライドするなら、有効なUsableParamを返す
[System.Serializable]
public class CpRoomUsableParamOverride
{
    public CpRoomUsableParam GetOverrideRoomUsableParam()
    {
        if (bOverride)
        {
            return _roomUsableParam;
        }
        return null;
    }
    [SerializeField]
    bool bOverride = false;
    [SerializeField, ShowIf(nameof(bOverride))]
    CpRoomUsableParam _roomUsableParam;

#if CP_DEBUG
    public bool DebugIsOverride { get => bOverride; set { bOverride = value; } }
    public CpRoomUsableParam DebugGetRoomUsableParam { get => _roomUsableParam; set { _roomUsableParam = value; } }
#endif
}