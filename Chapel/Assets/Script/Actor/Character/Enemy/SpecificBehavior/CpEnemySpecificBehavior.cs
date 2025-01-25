using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public abstract class CpEnemySpecificBehaviorBase
{
    public abstract void Apply(CpEnemyBase enemy);
}

public class CpEnemySpecificBehaviorStateGraph : CpEnemySpecificBehaviorBase
{
    public override void Apply(CpEnemyBase enemy)
    {
        enemy.SetStateGraph(_stateGraphAsset);
    }

    [SerializeField]
    StateGraphAsset _stateGraphAsset;
}

public class CpEnemySpecificBehaviorInitialYaw : CpEnemySpecificBehaviorBase
{
    public override void Apply(CpEnemyBase enemy)
    {
        enemy.transform.SetRotation(_initialYaw);
    }

    [SerializeField]
    float _initialYaw = 0f;
}