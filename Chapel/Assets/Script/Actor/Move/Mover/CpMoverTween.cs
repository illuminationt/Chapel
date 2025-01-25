using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ベクトルの意味
public enum ECpVectorType
{
    AbsoluteWorldPosition,
    AbsoluteScreenPosition,
    OffsetWorldSpace,
    OffsetScreenSpace,
}

public enum ECpOffsetType
{
    Absolute,
    Screen,
}

[Inspectable]
[System.Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamTween : ICpMoveParam
{
    public FCpMoveParamTween(in Vector2 vectorValue, float duration, ECpVectorType vectorType, Ease easingType)
    {
        VectorType = vectorType;
        VectorValue = vectorValue;
        Duration = duration;
        EasingType = easingType;
    }

    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.Tween;

    public void Inject(CpActorBase owner, CpMoveTweenTaskParamDirection paramDirection)
    {
        VectorValue = paramDirection.GetOffset(owner);
        VectorType = ECpVectorType.OffsetWorldSpace;
        Duration = paramDirection.Duration;
        EasingType = paramDirection.EasingType;
    }

    public Vector2 VectorValue;
    public ECpVectorType VectorType;
    public float Duration;
    public Ease EasingType;
}

public class CpMoverTween : CpMoverBase
{
    public static CpMoverTween Create(in FCpMoveParamTween param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverTween>(context);
        mover._paramTween = param;
        return mover;
    }

    protected override void InitializeInternal()
    {
        ICpTweenable tweenable = Context.OwnerActor;
        _tweenVector2Param = tweenable.GetTweenManager().GetUsableTweener<SltTweenVector2Param>();

        Vector2 deltaMove = CalcMoveVector();
        float duration = _paramTween.Duration;
        Ease easingType = _paramTween.EasingType;
        _activeTweener = _tweenVector2Param.StartMove(deltaMove, duration, easingType);

        _activeTweener.onComplete += () => { bActive = false; };
    }

    public override void GetPosValue(out ECpMoverPositionType outPosType, out Vector2 outValue)
    {
        outPosType = ECpMoverPositionType.DeltaPosition;
        outValue = _tweenVector2Param.GetDeltaMove();
    }
    public override void GetYawValue(out ECpMoverRotationType outYawType, out float outValue)
    {
        outYawType = ECpMoverRotationType.NoYaw;
        outValue = 0f;
    }

    protected override bool IsFinishedInternal()
    {
        if (bActive)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // ワールド空間での移動ベクトルを算出
    Vector2 CalcMoveVector()
    {
        Vector2 retVector = Vector2.zero;
        switch (_paramTween.VectorType)
        {
            case ECpVectorType.AbsoluteWorldPosition:
                {
                    Vector2 ownerPosition = Context.InitialOwnerPosition;
                    Vector2 deltaMove = _paramTween.VectorValue - ownerPosition;
                    retVector = deltaMove;
                }
                break;
            case ECpVectorType.AbsoluteScreenPosition:
                retVector = CpUtil.GetWorldPositionFromNormalizedPosition(_paramTween.VectorValue);
                break;
            case ECpVectorType.OffsetWorldSpace:
                retVector = _paramTween.VectorValue;
                break;
            case ECpVectorType.OffsetScreenSpace:
                retVector = CpUtil.CalcDeltaVector(_paramTween.VectorValue, ECpOffsetType.Screen);
                break;
        }

        return retVector;
    }

    FCpMoveParamTween _paramTween;
    SltTweenVector2Param _tweenVector2Param;
    Tweener _activeTweener = null;
    bool bActive = true;
}
