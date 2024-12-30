using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CpActorBase))]
public class CpHellComponent : MonoBehaviour
{
    public CpHellComponent()
    {
    }
    private void Update()
    {
        for (int index = _multiHellUpdators.Count - 1; index >= 0; index--)
        {
            CpMultiHellUpdator updator = _multiHellUpdators[index];
            updator.Update();
        }
    }

    public void RequestStart(CpHellParam hellParam)
    {
        CpMultiHellParam newMultiHellParam = new CpMultiHellParam();
        CpMultiHellParamElement element = new CpMultiHellParamElement();
        element.ParamElement = hellParam;
        newMultiHellParam.ParamElements.Add(element);

        RequestStart(newMultiHellParam);
    }

    public void RequestStart(CpMultiHellParam multiHellParam)
    {
        FCpUpdateHellContext context;
        context.Position = transform.position;

        ICpActorForwardInterface forwardInterface = GetComponent<CpActorBase>();
        context.Degree = forwardInterface.GetForwardDegree();

        CpMultiHellUpdator newMultiUpdator = new CpMultiHellUpdator(multiHellParam, context);
        newMultiUpdator.Start();
        _multiHellUpdators.Add(newMultiUpdator);
    }
    List<CpMultiHellUpdator> _multiHellUpdators = new List<CpMultiHellUpdator>();
}
