using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CpActorBase : MonoBehaviour,
    ICpActorForwardInterface,
    ICpTweenable
{
    public virtual float GetForwardDegree()
    {
        Assert.IsTrue(false);
        return 0f;
    }

    // ICpTweenable
    public SltTweenManager GetTweenManager()
    {

        if (_tweenManager == null)
        {
            _tweenManager = new SltTweenManager();
        }
        return _tweenManager;
    }

    SltTweenManager _tweenManager = null;
}
