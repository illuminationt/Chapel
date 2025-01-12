using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

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

            if (updator.IsFinished())
            {
                FCpMultiHellUpdatorId id = updator.GetId();
                _multiHellUpdators.RemoveAt(index);
                OnHellFinished.Invoke(id);
            }
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

    public FCpMultiHellUpdatorId RequestStart(CpMultiHellParam multiHellParam)
    {
        FCpUpdateHellContext context;
        context.RootTransform = transform;
        context.InitialPosition = transform.position;

        ICpActorForwardInterface forwardInterface = GetComponent<CpActorBase>();
        context.InitialDegree = forwardInterface.GetForwardDegree();

        CpMultiHellUpdator newMultiUpdator = new CpMultiHellUpdator(multiHellParam, context);
        newMultiUpdator.Start();
        _multiHellUpdators.Add(newMultiUpdator);

        return newMultiUpdator.GetId();
    }

    public UnityEvent<FCpMultiHellUpdatorId> OnHellFinished => _onHellFinished;

    List<CpMultiHellUpdator> _multiHellUpdators = new List<CpMultiHellUpdator>();
    UnityEvent<FCpMultiHellUpdatorId> _onHellFinished = new UnityEvent<FCpMultiHellUpdatorId>();
}
