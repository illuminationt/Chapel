using UnityEngine;

public class CpEnemyRotateGuardShield : CpActorBase
{
    public override ECpMoverUpdateType GetMoverUpdateType()
    {
        return ECpMoverUpdateType.UpdateFunction;
    }

    public override float GetForwardDegree()
    {
        return 0f;
    }
}
