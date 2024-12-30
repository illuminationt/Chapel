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

public class CpMultiHellUpdator
{
    public CpMultiHellUpdator(CpMultiHellParam multiHellParam, in FCpUpdateHellContext context)
    {
        _timer = 0f;
        _multiHellParam = multiHellParam;
        _updators = new List<CpHellUpdator>(multiHellParam.ParamElements.Count);

        foreach (CpMultiHellParamElement element in multiHellParam.ParamElements)
        {
            CpHellUpdator newUpdator = new CpHellUpdator(element.ParamElement, context);
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

    float _timer = 0f;
    CpMultiHellParam _multiHellParam = null;
    List<CpHellUpdator> _updators;
}