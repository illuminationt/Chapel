using ImGuiNET;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

[Inspectable]
[System.Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamTrampoline : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType() => ECpMoveParamType.Trampoline;

    [Inspectable] public float JumpSpeed;
    [Inspectable] public float JumpAngle;
    [Inspectable] public float Gravity;
}

// ƒgƒ‰ƒ“ƒ|ƒŠƒ“ˆÚ“®—p
public class CpMoverEnemyTrampoline : CpMoverBase
{
    public static CpMoverEnemyTrampoline Create(in FCpMoveParamTrampoline param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverEnemyTrampoline>(context);
        mover._param = param;
        return mover;
    }
    protected override void InitializeInternal()
    {
        float currentYaw = Context.InitialOwnerDegree;
        float jumpAngle = currentYaw + _param.JumpAngle;
        _currentVelocity = _param.JumpSpeed * SltMath.ToVector(jumpAngle);

        float gravityDeg = SltMath.UnwindDegree(currentYaw + 90f);
        _gravityDirection = SltMath.ToVector(gravityDeg);

        _currentYaw = Context.InitialOwnerDegree;

        _currentState = ECpState.Jump;
    }
    protected override void UpdateInternal()
    {
        switch (_currentState)
        {
            case ECpState.Jump:
                _currentVelocity -= _gravityDirection * _param.Gravity * CpTime.DeltaTime;
                break;
            case ECpState.CollideWall:
            case ECpState.AppliedCollideDeg:
                _currentVelocity = Vector2.zero;
                break;
        }
    }

    public override void GetPosValue(out ECpMoverPositionType outPosType, out Vector2 outValue)
    {
        outPosType = ECpMoverPositionType.Velocity;
        outValue = _currentVelocity;
    }

    public override void GetYawValue(out ECpMoverRotationType outYawType, out float outValue)
    {
        outYawType = ECpMoverRotationType.AbsoluteYaw;
        outValue = _currentYaw;
    }
    protected override bool IsFinishedInternal()
    {
        return _currentState == ECpState.AppliedCollideDeg;
    }


    public override void OnCollisionEnterHit2D(Collision2D collision)
    {
        _currentState = ECpState.CollideWall;

        Vector2 impactNormal = collision.contacts[0].normal;
        float normalDeg = SltMath.ToDegree(impactNormal);

        _currentYaw = normalDeg - 90f;
    }

    public override void OnMoverValueApplied()
    {
        if (_currentState == ECpState.CollideWall)
        {
            _currentState = ECpState.AppliedCollideDeg;
        }
    }

    Vector2 _currentVelocity = Vector2.zero;
    Vector2 _gravityDirection = Vector2.zero;
    float _currentYaw = 0f;

    enum ECpState
    {
        None,
        Jump,
        CollideWall,
        AppliedCollideDeg,
    }
    ECpState _currentState = ECpState.None;

    FCpMoveParamTrampoline _param;
}
