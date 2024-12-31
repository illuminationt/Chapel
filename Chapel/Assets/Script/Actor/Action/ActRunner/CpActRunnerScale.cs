using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECpActScaleType
{
    XY = 0,
    X = 1,
    Y = 2,
}

[System.Serializable]
public struct FCpActParamScale : ICpActionParam
{
    public ECpActionParamType GetActionParamType() { return ECpActionParamType.Scale; }

    public ECpActScaleType ScaleType;
    public Ease EasingType;
    public float TargetScale;
    public float Duration;
}
public class CpActRunnerScale : CpActRunnerBase
{
    public static CpActRunnerScale Create(in FCpActParamScale param, in FCpActRunnerContext context)
    {
        var runner = CreateActRunner<CpActRunnerScale>(context);
        runner._param = param;
        return runner;
    }

    protected override void InitializeInternal()
    {
        ICpTweenable tweenable = Context.OwnerActor;
        _tweenFloatParam = tweenable.GetUsableTweener<SltTweenFloatParam>();

        float initialScale = Context.OwnerActor.transform.localScale.x;
        float targetScale = _param.TargetScale;
        _activeTweener = _tweenFloatParam.StartTween(initialScale, targetScale, _param.Duration, _param.EasingType);
        _activeTweener.onComplete += () => { _bActive = false; };
    }

    protected override void UpdateInternal()
    {
        float currentScale = _tweenFloatParam.GetFloatValue();
        Context.OwnerActor.transform.localScale = Vector3.one * currentScale;
    }

    public override bool IsFinished()
    {
        if (_bActive) { return false; }
        else { return true; }
    }

    FCpActParamScale _param;
    SltTweenFloatParam _tweenFloatParam;
    Tweener _activeTweener = null;
    bool _bActive = true;
}
