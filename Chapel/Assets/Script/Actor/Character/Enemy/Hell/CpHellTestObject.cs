using UnityEngine;

[RequireComponent(typeof(CpHellComponent))]
public class CpHellTestObject : CpActorBase
{
    CpHellComponent _hellComponent = null;

    protected override void Awake()
    {
        base.Awake();
        _hellComponent = GetComponent<CpHellComponent>();
    }

    // ICpActorForwardInterface
    public override float GetForwardDegree()
    {
        return SltMath.ToDegree(Vector2.down);
    }

    // end of ICpActorForwardInterface


    public void RequestStartHell(CpHellParamScriptableObject paramSO)
    {
        _hellComponent.RequestStart(paramSO.MultiHellParam);
    }
}
