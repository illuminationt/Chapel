using ImGuiNET;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

// Moverが返す座標変化値の種類
public enum ECpMoverPositionType
{
    None = 0,// 不正値
    NoPos,// 座標移動しない
    DeltaPosition,// 座標のデルタ値
    Velocity,// 速度
}

public enum ECpMoverRotationType
{
    None = 0,// 不正値
    NoYaw,// Yaw回転しない
    RotationSpeed,// Yaw回転スピード deg/s
    DeltaYaw,// Yaw回転のデルタ値
    AbsoluteYaw,// Yaw直値
}


public struct FCpMoverId
{
    public static FCpMoverId Create()
    {
        FCpMoverId newId = new FCpMoverId();
        if (LatestId > UInt64.MaxValue - 5)
        {
            LatestId = 1;
        }
        newId._id = LatestId++;
        return newId;
    }

    public bool Equals(in FCpMoverId other)
    {
        if (!IsValid() || !other.IsValid())
        {
            return false;
        }
        return _id == other._id;
    }

    public void Reset()
    {
        this = INVALID_ID;
    }

    public string ToString()
    {
        return _id.ToString();
    }
    bool IsValid()
    {
        if (_id != 0) { return true; } else { return false; }
    }
    public static FCpMoverId INVALID_ID;
    static UInt64 LatestId = 1;
    UInt64 _id;
}

public abstract class CpMoverBaseUtility
{
    protected static T CreateMover<T>(in FCpMoverContext context) where T : CpMoverBase
    {
        T mover = (T)Activator.CreateInstance(typeof(T));
        mover.InitializeBaseParam(context);
        return mover;
    }
    protected void InitializeBaseParam(in FCpMoverContext context)
    {
        _context = context;
        _moverId = FCpMoverId.Create();
    }

    protected virtual void InitializeInternal()
    {

    }

    public void Update()
    {
        if (!bInitialized)
        {
            InitializeInternal();
            bInitialized = true;
        }

        UpdateInternal();
        _duration += CpTime.DeltaTime;
    }

    public void Reset()
    {
        _context.Reset();
        _duration = 0f;
        bInitialized = false;
        _moverId.Reset();
        ResetInternal();
    }
    public void ExternalCancel()
    {
        bCancelledExternally = true;
    }
    private void ResetInternal() { }

    protected virtual void UpdateInternal() { }

    public abstract void GetPosValue(out ECpMoverPositionType outPosType, out Vector2 outValue);

    public abstract void GetYawValue(out ECpMoverRotationType outYawType, out float outValue);

    public bool IsFinished()
    {
        if (bCancelledExternally)
        {
            return true;
        }
        return IsFinishedInternal();
    }
    protected virtual bool IsFinishedInternal() { return false; }


    // コールバック
    public virtual void OnCollisionEnterHit2D(Collision2D collision) { }

    public virtual void OnMoverValueApplied() { }


    public FCpMoverId GetId() => _moverId;

    // 計算
    public Vector2 GetDeltaMove()
    {
        ECpMoverPositionType posType = ECpMoverPositionType.None;
        Vector2 vectorValue = Vector2.zero;
        GetPosValue(out posType, out vectorValue);

        switch (posType)
        {
            case ECpMoverPositionType.NoPos:
                return Vector2.zero;
            case ECpMoverPositionType.DeltaPosition:
                return vectorValue;
            case ECpMoverPositionType.Velocity:
                return vectorValue * CpTime.SmoothDeltaTime;
            default:
                Assert.IsTrue(false);
                return Vector2.zero;
        }
    }

    public float GetDeltaYaw(float currentYaw)
    {
        ECpMoverRotationType rotType = ECpMoverRotationType.None;
        float yawValue = 0f;
        GetYawValue(out rotType, out yawValue);
        switch (rotType)
        {
            case ECpMoverRotationType.NoYaw:
                return 0f;
            case ECpMoverRotationType.RotationSpeed:
                return yawValue * CpTime.DeltaTime;
            case ECpMoverRotationType.DeltaYaw:
                return yawValue;
            case ECpMoverRotationType.AbsoluteYaw:
                return SltMath.UnwindAngle(yawValue - currentYaw);
            default:
                Assert.IsTrue(false);
                return 0f;
        }
    }

    protected ref readonly CpMoverManager OwnerMoverManager => ref _context.OwnerMoverManager;
    protected ref readonly FCpMoverContext Context => ref _context;
    protected float Duration => _duration;

    protected Vector2 GetCurrentOwnerActorPosition() => Context.OwnerActor.transform.position;
    protected Vector2 GetCurrentOwnerVelocity() => OwnerMoverManager.GetVelocity();

    // 
    FCpMoverContext _context;
    float _duration = 0f;
    bool bInitialized = false;
    bool bCancelledExternally = false;
    FCpMoverId _moverId;

#if CP_DEBUG
    public virtual void DrawImGui()
    {
        string durationStr = $"{_duration}(s)";
        ImGui.Text(durationStr);

        //Vector2 deltaMove = GetDeltaMove();
        //Vector2 velocity = GetVelocity();
        //string deltaMoveStr = $"Δ=({deltaMove.x},{deltaMove.y})";
        //ImGui.Text(deltaMoveStr);
        //string velStr = $"Velocity = ({velocity.x},{velocity.y})";
        //ImGui.Text(velStr);
    }
#endif
}

// 念のため階層を挟んでおく
public abstract class CpMoverBase : CpMoverBaseUtility
{

}

