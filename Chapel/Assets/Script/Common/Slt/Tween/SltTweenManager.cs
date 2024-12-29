using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TweenVector = DG.Tweening.Core.TweenerCore<UnityEngine.Vector2, UnityEngine.Vector2, DG.Tweening.Plugins.Options.VectorOptions>;
using TweenFloat = DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions>;
using TweenPath = DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions>;
using DGPath = DG.Tweening.Plugins.Core.PathCore.Path;
using UnityEngine.Events;
using System;
using UnityEngine.Assertions;

public class SltTweenParamBase
{
    protected Transform MyTransform;
    public virtual Tweener GetTweener() { return null; }
    public bool IsPlaying()
    {
        Tweener t = GetTweener();
        if (!t.IsActive())
        {
            return false;
        }
        return t.IsPlaying();
    }
    public void Kill()
    {
        GetTweener().Kill();
    }
}

[System.Serializable]
public class SltTweenVector2Param : SltTweenParamBase
{
    // ˆÚ“®
    TweenVector TweenerVector = null;
    Vector2 _tweeningVector = Vector2.zero;
    Vector2 _prevTweeningVector = Vector2.zero;
    Vector2 _latestDeltaMove = Vector2.zero;

    UnityAction<Vector2> _onUpdate;

    public virtual TweenVector StartMove(
        in Vector2 MoveVector, float Duration, Ease EasingType, UnityAction<Vector2> onUpdate)
    {
        _tweeningVector = Vector2.zero;
        _prevTweeningVector = Vector2.zero;
        _onUpdate = onUpdate;

        TweenerVector = DOTween.To(() => _tweeningVector, (x) => _tweeningVector = x, MoveVector, Duration);
        TweenerVector.SetEase(EasingType);
        TweenerVector.OnUpdate(() => { OnUpdateTweening(); });
        TweenerVector.OnComplete(() => { OnCompleteTweening(); });

        return TweenerVector;
    }

    public TweenVector StartMove(in Vector2 moveVector, float duration, Ease easingType)
    {
        return StartMove(moveVector, duration, easingType, null);
    }

    protected virtual void OnUpdateTweening()
    {
        _latestDeltaMove = _tweeningVector - _prevTweeningVector;
        _onUpdate?.Invoke(_latestDeltaMove);
        _prevTweeningVector = _tweeningVector;
    }

    void OnCompleteTweening()
    {
        _tweeningVector = Vector2.zero;
        _prevTweeningVector = Vector2.zero;
        _latestDeltaMove = Vector2.zero;
        _onUpdate = null;
    }

    public override Tweener GetTweener()
    {
        return TweenerVector;
    }

    public Vector2 GetDeltaMove() => _latestDeltaMove;
}


[System.Serializable]
public class SltTweenFloatParam : SltTweenParamBase
{
    TweenFloat TweenerFloat = null;
    protected float TweeningSpeed = 0f;
    UnityAction<float> _onUpdateFloat = null;
    public TweenFloat StartMove(
       float initialFloat, float lastSpeed, float duration, Ease easingType, UnityAction<float> onUpdate)
    {
        TweeningSpeed = initialFloat;
        _onUpdateFloat = onUpdate;

        TweenerFloat = DOTween.To(() => TweeningSpeed, (x) => TweeningSpeed = x, lastSpeed, duration);
        TweenerFloat.SetEase(easingType);
        TweenerFloat.OnUpdate(() => { OnUpdateTweenerSpeed(); });
        TweenerFloat.OnComplete(() => { OnCompleteTweenerSpeed(); });

        return TweenerFloat;
    }

    void OnUpdateTweenerSpeed()
    {
        _onUpdateFloat.Invoke(TweeningSpeed);
    }

    void OnCompleteTweenerSpeed()
    {
        TweeningSpeed = 0f;
        _onUpdateFloat = null;
    }
    public override Tweener GetTweener()
    {
        return TweenerFloat;
    }
}

public class SltTweenManager
{
    List<SltTweenFloatParam> tweenFloatParamList = new List<SltTweenFloatParam>();
    List<SltTweenVector2Param> tweenVector2ParamList = new List<SltTweenVector2Param>();

    static System.Type TweenFloatType = typeof(SltTweenFloatParam);
    static System.Type TweenVector2Type = typeof(SltTweenVector2Param);

    public List<T> GetList<T>() where T : SltTweenParamBase
    {
        System.Type type = typeof(T);
        if (type == TweenFloatType)
        {
            return tweenFloatParamList as List<T>;
        }
        else if (type == TweenVector2Type)
        {
            return tweenVector2ParamList as List<T>;
        }
        else
        {
            Assert.IsTrue(false);
            return null;
        }
    }
    public T GetUsableTweener<T>() where T : SltTweenParamBase, new()
    {
        List<T> list = GetList<T>();
        foreach (SltTweenParamBase Param in list)
        {
            if (!Param.IsPlaying())
            {
                return Param as T;
            }
        }

        T t = new T();
        list.Add(t);
        return t;
    }
}
