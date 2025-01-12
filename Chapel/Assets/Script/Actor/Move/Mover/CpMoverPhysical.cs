using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public struct FCpMoveParamPhysical : ICpMoveParam
{
    public ECpMoveParamType GetMoveParamType()
    {
        return ECpMoveParamType.Physical;
    }

    public float Speed;
    public float Accel;
    public float DegreeOffset;
    public float RestitutionCoefficient;
}

public class CpMoverPhysical : CpMoverBase
{
    public static CpMoverPhysical Create(in FCpMoveParamPhysical param, in FCpMoverContext context)
    {
        var mover = CreateMover<CpMoverPhysical>(context);
        mover._param = param;
        return mover;
    }

    protected override void InitializeInternal()
    {
        _currentSpeed = _param.Speed;
        _initialDegree = Context.InitialOwnerDegree + _param.DegreeOffset;
        _currentDegree = _initialDegree;
        _ownerTransform = Context.OwnerActor.transform;
    }

    protected override void UpdateInternal()
    {
        _currentSpeed += _param.Accel * CpTime.DeltaTime;
        if (_currentSpeed < 0f)
        {
            _currentSpeed = 0f;
        }
    }

    public override Vector2 GetVelocity()
    {
        Vector2 dir = SltMath.ToVector(_currentDegree);
        return dir * _currentSpeed;
    }
    public override bool IsFinished() { return _currentSpeed == 0f; }


    public override void OnCollisionEnterHit2D(Collision2D collision)
    {
        Assert.IsTrue(collision.contacts.Length > 0);

        // 壁に衝突した後の角度を算出
        // まず法線方向を取得
        ContactPoint2D contactPoint = collision.contacts[0];
        Vector2 impactNormal = ((Vector2)_ownerTransform.position - contactPoint.point).normalized;

        Vector2 currentDir = SltMath.ToVector(_currentDegree);

        // 跳ね返った後の角度を算出
        Vector2 reflectedDir = Vector2.Reflect(currentDir, impactNormal);
        _currentDegree = SltMath.ToDegree(reflectedDir);


        // 壁に衝突した後のスピードを算出
        _currentSpeed *= _param.RestitutionCoefficient;
    }

    float _currentSpeed = 0f;
    float _initialDegree = 0f;
    float _currentDegree = 0f;

    Transform _ownerTransform = null;
    FCpMoveParamPhysical _param;
}
