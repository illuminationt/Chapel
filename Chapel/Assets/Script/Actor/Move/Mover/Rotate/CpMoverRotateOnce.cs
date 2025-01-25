using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Unity.VisualScripting;
using NUnit.Framework;
using System;
using static DG.Tweening.DOTweenAnimation;

// âÒì]äpìxÇÃåàíËï˚ñ@
public enum ECpRotateDegreeType
{
    None = 0,
    FixedYaw = 1,// âÒì]ílÇ™å≈íËílÇ…Ç»ÇÈÇÊÇ§Ç…âÒì]
    DeltaYaw = 2,// éwíËÇµÇΩäpìxÇæÇØâÒì]
    Face = 3,// éwíËÇµÇΩç¿ïWÇå¸Ç≠ÇÊÇ§Ç…âÒì]
}

public enum ECpRotateTargetType
{
    None = 0,
    Player = 1,
}

public static class CpRotateTargetUtil
{
    public static bool GetTargetLocation(ECpRotateTargetType targetType, ref Vector2 refLocation)
    {
        switch (targetType)
        {
            case ECpRotateTargetType.Player:
                refLocation = CpGameManager.Instance.Player.transform.position;
                return true;
            default:
                Assert.IsTrue(false);
                return false;
        }
    }
}

[Inspectable]
[System.Serializable]
public class CpRotateTargetParam
{
    public float CalcTargetRotZ(CpActorBase ownerActor)
    {
        switch (RotateType)
        {
            case ECpRotateDegreeType.FixedYaw:
                return GoalYaw;
            case ECpRotateDegreeType.DeltaYaw:
                {
                    float ownerRotZ = ownerActor.transform.eulerAngles.z;
                    return ownerRotZ + DeltaYaw;
                }
            case ECpRotateDegreeType.Face:
                return CalcTargetRotZ_Face(ownerActor);
            default:
                Assert.IsTrue(false);
                return 0f;
        }
    }

    float CalcTargetRotZ_Face(CpActorBase ownerActor)
    {
        Vector2 faceTargetPosition = GetFaceTargetPosition();
        Vector2 selfPosition = ownerActor.transform.position;

        float rotZ = SltMath.CalcFaceRotationZ(selfPosition, faceTargetPosition);
        return SltMath.UnwindAngle(rotZ + FaceOffsetYaw);
    }
    Vector2 GetFaceTargetPosition()
    {
        Vector2 target = Vector2.zero;
        CpRotateTargetUtil.GetTargetLocation(TargetType, ref target);
        return target;
    }

    // inject
    public void Inject(CpRotateTaskParamBase paramBase)
    {
        switch (paramBase)
        {
            case CpRotateTaskParamFixedYaw fix:
                InjectFixedYaw(fix);
                break;
            case CpRotateTaskParamDeltaYaw deltaYaw:
                InjectDeltaYaw(deltaYaw);
                break;
            case CpRotateTaskParamFace face:
                InjectFace(face);
                break;
            default:
                Assert.IsTrue(false);
                break;
        }
    }
    void InjectFixedYaw(CpRotateTaskParamFixedYaw paramFixedYaw)
    {
        RotateType = ECpRotateDegreeType.FixedYaw;
        GoalYaw = paramFixedYaw.GoalYaw;
    }
    void InjectDeltaYaw(CpRotateTaskParamDeltaYaw paramDeltaYaw)
    {
        RotateType = ECpRotateDegreeType.DeltaYaw;
        DeltaYaw = paramDeltaYaw.DeltaYaw;
    }
    void InjectFace(CpRotateTaskParamFace paramFace)
    {
        RotateType = ECpRotateDegreeType.Face;
        TargetType = paramFace.TargetType;
        FaceOffsetYaw = paramFace.OffsetYaw;
    }

    public bool IsValidParam()
    {
        switch (RotateType)
        {
            case ECpRotateDegreeType.FixedYaw:
                return true;
            case ECpRotateDegreeType.DeltaYaw:
                return true;
            case ECpRotateDegreeType.Face:
                return TargetType != ECpRotateTargetType.None;
            default:
                Assert.IsTrue(false);
                return false;
        }
    }

    [Inspectable, SerializeField] ECpRotateDegreeType RotateType;
    [Inspectable, SerializeField]
    [ShowIf("RotateType", ECpRotateDegreeType.FixedYaw)]
    float GoalYaw = 0f;

    [Inspectable, SerializeField]
    [ShowIf("RotateType", ECpRotateDegreeType.DeltaYaw)]
    float DeltaYaw = 0f;

    // Face
    [Inspectable, SerializeField]
    [ShowIf("RotateType", ECpRotateDegreeType.Face)]
    ECpRotateTargetType TargetType;
    [Inspectable, SerializeField]
    [ShowIf("RotateType", ECpRotateDegreeType.Face)]
    float FaceOffsetYaw = 0f;
}

[Inspectable]
[Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamRotate : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType()
    {
        return ECpMoveParamType.Rotate;
    }

    public float GetTargetRotZ(CpActorBase ownerActor)
    {
        return targetParam.CalcTargetRotZ(ownerActor);
    }

    public void Inject(CpRotateTaskParamFixedYaw paramFixedYaw)
    {
        InjectInternal(paramFixedYaw);
    }
    public void Inject(CpRotateTaskParamDeltaYaw paramDeltaYaw)
    {
        InjectInternal(paramDeltaYaw);
        // targetParam.Inject(paramDeltaYaw);
    }
    public void Inject(CpRotateTaskParamFace paramFace)
    {
        InjectInternal(paramFace);
        //targetParam.Inject(paramFace);
    }
    void InjectInternal(CpRotateTaskParamBase param)
    {
        // targetParaméwíËçœÇ›Ç»ÇÁInjectÇÕésì‡ëzíË
        Assert.IsTrue(targetParam == null);
        targetParam = new CpRotateTargetParam();
        targetParam.Inject(param);

        Speed = param.Speed;
        EasingType = param.EasingType;
    }

    public bool IsValidParam()
    {
        if (Speed <= 0f) { return false; }
        if (EasingType == Ease.Unset) { return false; }
        if (targetParam == null || !targetParam.IsValidParam()) { return false; }

        return true;
    }

    [Inspectable, SerializeField] CpRotateTargetParam targetParam;
    [Inspectable, SerializeField] public float Speed;
    [Inspectable, SerializeField] public Ease EasingType;
}

// ñ⁄ïWï˚å¸Ç…å¸Ç©Ç¡ÇƒâÒì]
// ç≈èâÇ…åàÇﬂÇΩâÒì]ï˚å¸Ç…å¸Ç´èIÇÌÇ¡ÇΩÇÁèIóπ
public class CpMoverRotateOnce : CpMoverBase
{
    public static CpMoverRotateOnce Create(in FCpMoveParamRotate param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverRotateOnce>(context);
        mover._paramRotate = param;
        return mover;
    }

    protected override void InitializeInternal()
    {
        ICpTweenable tweenable = Context.OwnerActor;
        _tweenFloatParam = tweenable.GetTweenManager().GetUsableTweener<SltTweenFloatParam>();

        _initialRotZ = Context.OwnerActor.transform.eulerAngles.z;
        float targetRotZ = _paramRotate.GetTargetRotZ(Context.OwnerActor);

        float deltaRotZ = SltMath.GetRotateDelta(_initialRotZ, targetRotZ, 1f);

        float duration = Mathf.Abs(deltaRotZ) / _paramRotate.Speed;
        _activeTweener = _tweenFloatParam.StartTween(0f, deltaRotZ, duration, _paramRotate.EasingType);

        _activeTweener.onComplete += () => { bActive = false; };
    }

    public override void GetPosValue(out ECpMoverPositionType outPosType, out Vector2 outValue)
    {
        outPosType = ECpMoverPositionType.NoPos;
        outValue = Vector2.zero;
    }
    public override void GetYawValue(out ECpMoverRotationType outYawType, out float outValue)
    {
        outYawType = ECpMoverRotationType.DeltaYaw;
        outValue = _tweenFloatParam.GetDeltaValue();
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

    FCpMoveParamRotate _paramRotate;
    SltTweenFloatParam _tweenFloatParam;
    Tweener _activeTweener = null;
    float _initialRotZ = 0f;
    bool bActive = true;
}
