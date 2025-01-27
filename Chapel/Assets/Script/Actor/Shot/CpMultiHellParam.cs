using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CpMultiHellParamElement
{
    public CpHellParam ParamElement;
    public float Delay = 0f;
}

[System.Serializable]
public class CpMultiHellParam
{
    public List<CpMultiHellParamElement> ParamElements;
}

public struct FCpMultiHellUpdatorId
{

    public static FCpMultiHellUpdatorId Create()
    {
        FCpMultiHellUpdatorId newId;
        if (LatestId > UInt64.MaxValue - 4)
        {
            LatestId = 0;
        }

        newId._id = ++LatestId;
        return newId;
    }

    public bool Equals(in FCpMultiHellUpdatorId otherId)
    {
        return _id == otherId._id;
    }

    static UInt64 LatestId = 0;
    UInt64 _id;
}

public class CpMultiHellUpdator
{
    public CpMultiHellUpdator(CpMultiHellParam multiHellParam, CpHellRequestOption option, in FCpUpdateHellContext context)
    {
        _timer = 0f;
        _id = FCpMultiHellUpdatorId.Create();
        _multiHellParam = multiHellParam;
        _option = option;
        _updators = new List<CpHellUpdator>(multiHellParam.ParamElements.Count);

        foreach (CpMultiHellParamElement element in multiHellParam.ParamElements)
        {
            CpHellUpdator newUpdator = new CpHellUpdator(element.ParamElement, option, context);
            _updators.Add(newUpdator);
        }
    }

    public void Start()
    {

    }

    public void Update()
    {
        _timer += CpTime.DeltaTime;
        foreach (CpHellUpdator updator in _updators)
        {
            updator.Update();
        }
    }

    public bool IsFinished()
    {

        foreach (CpHellUpdator updator in _updators)
        {
            if (!updator.IsFinished())
            {
                return false;
            }
        }
        return true;
    }

    public FCpMultiHellUpdatorId GetId() { return _id; }

    float _timer = 0f;
    CpMultiHellParam _multiHellParam = null;
    CpHellRequestOption _option = null;
    List<CpHellUpdator> _updators;
    FCpMultiHellUpdatorId _id;
}