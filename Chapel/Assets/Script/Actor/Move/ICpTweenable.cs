using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICpTweenable
{
    public SltTweenManager GetTweenManager();

    public T GetUsableTweener<T>() where T : SltTweenParamBase, new()
    {
        return GetTweenManager().GetUsableTweener<T>();
    }

}
