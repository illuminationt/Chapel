using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICpActorForwardInterface
{
    public abstract float GetForwardDegree();
    public Vector2 GetForwardVector()
    {
        return SltMath.ToVector(GetForwardDegree());
    }
}
