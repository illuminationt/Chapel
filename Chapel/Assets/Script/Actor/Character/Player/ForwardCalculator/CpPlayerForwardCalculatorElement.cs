using UnityEngine;

public abstract class CpPlayerForwardCalculatorElementBase
{
    public abstract ECpPlayerForwardType GetForwardType();
    public Vector2 GetForwardVector() { return Vector2.zero; }
    public virtual void Update()
    {

    }

    protected virtual Vector2 CalcFocalLocation()
    {
        return Vector2.zero;
    }
}
