using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CpMoverBaseUtility
{
    protected void InitializeBaseParam(FCpMoverContext context)
    {
        _context = context;
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
    protected virtual void UpdateInternal() { }
    public virtual Vector2 GetDeltaMove() { return GetVelocity() * Time.smoothDeltaTime; }
    public abstract Vector2 GetVelocity();

    protected float Duration => _duration;

    // 
    FCpMoverContext _context;
    float _duration = 0f;
    bool bInitialized = false;
}

// ”O‚Ì‚½‚ßŠK‘w‚ð‹²‚ñ‚Å‚¨‚­
public abstract class CpMoverBase : CpMoverBaseUtility
{
    protected static T CreateMover<T>(in FCpMoverContext context) where T : CpMoverBase
    {
        T mover = (T)Activator.CreateInstance(typeof(T));
        mover.InitializeBaseParam(context);
        return mover;
    }


    //public static CpMoverBase Create(CpMoveParamBase moveParam)
    //{
    //    System.Type moverType = moveParam.GetOwnerMoverType();
    //    CpMoverBase mover = (CpMoverBase)Activator.CreateInstance(moverType);
    //    return mover;
    //}
}

