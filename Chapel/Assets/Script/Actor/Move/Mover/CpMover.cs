using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

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
    private void ResetInternal() { }

    protected virtual void UpdateInternal() { }
    public virtual Vector2 GetDeltaMove() { return GetVelocity() * Time.smoothDeltaTime; }
    public abstract Vector2 GetVelocity();
    public virtual bool IsFinished() { return false; }

    public virtual void OnCollisionEnterHit2D(Collision2D collision) { }

    public FCpMoverId GetId() => _moverId;

    protected ref readonly CpMoverManager OwnerMoverManager => ref _context.OwnerMoverManager;
    protected ref readonly FCpMoverContext Context => ref _context;
    protected float Duration => _duration;

    protected Vector2 GetCurrentOwnerActorPosition() => Context.OwnerActor.transform.position;
    protected Vector2 GetCurrentOwnerVelocity() => OwnerMoverManager.GetVelocity();

    // 
    FCpMoverContext _context;
    float _duration = 0f;
    bool bInitialized = false;
    FCpMoverId _moverId;
}

// ”O‚Ì‚½‚ßŠK‘w‚ð‹²‚ñ‚Å‚¨‚­
public abstract class CpMoverBase : CpMoverBaseUtility
{

}

