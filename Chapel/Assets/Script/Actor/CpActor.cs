using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CpActorBase : MonoBehaviour,
    ICpActorForwardInterface
{
    public virtual float GetForwardDegree()
    {
        Assert.IsTrue(false);
        return 0f;
    }
}
