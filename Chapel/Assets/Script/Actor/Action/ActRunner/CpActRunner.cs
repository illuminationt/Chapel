using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FCpActRunnerId
{
    public static FCpActRunnerId Create()
    {
        FCpActRunnerId newRunnerId = new FCpActRunnerId();

        FCpInstanceId newInstanceId = FCpInstanceId.Create();
        newRunnerId._id = newInstanceId;
        return newRunnerId;
    }
    public bool Equals(in FCpActRunnerId id)
    {
        return id._id.Equals(_id);
    }

    public static FCpActRunnerId INVALID_ID;

    FCpInstanceId _id;
}

public abstract class CpActRunnerBase
{
    protected static T CreateActRunner<T>(in FCpActRunnerContext context) where T : CpActRunnerBase
    {
        T runner = (T)Activator.CreateInstance(typeof(T));
        runner.InitializeBaseParam(context);
        return runner;
    }

    protected void InitializeBaseParam(in FCpActRunnerContext context)
    {
        _context = context;
    }
    protected virtual void InitializeInternal() { }

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

    protected virtual void UpdateInternal()
    {
    }

    public void OnFinished()
    {
        OnFinishedInternal();
    }

    protected virtual void OnFinishedInternal() { }

    public virtual bool IsFinished() { return false; }

    public void Reset()
    {
        _context.Reset();
        _duration = 0f;
        bInitialized = false;
        ResetInternal();
    }
    protected virtual void ResetInternal() { }

    public FCpActRunnerId GetId() => _runnerId;
    protected ref readonly FCpActRunnerContext Context => ref _context;
    FCpActRunnerContext _context;
    float _duration;
    bool bInitialized = false;
    FCpActRunnerId _runnerId;
}
