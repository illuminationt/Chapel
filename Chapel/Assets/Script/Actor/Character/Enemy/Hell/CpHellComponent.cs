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

    public void RequestStart(CpHellParam hellParam, CpHellRequestOption option)
    {
        CpMultiHellParam newMultiHellParam = new CpMultiHellParam();
        CpMultiHellParamElement element = new CpMultiHellParamElement();
        element.ParamElement = hellParam;
        newMultiHellParam.ParamElements.Add(element);

        RequestStart(newMultiHellParam, option);
    }

    public FCpMultiHellUpdatorId RequestStart(CpMultiHellParam multiHellParam, CpHellRequestOption option)
    {
        FCpUpdateHellContext context;
        context.RootTransform = transform;
        context.InitialPosition = transform.position;

        context.ForwardInterface = GetComponent<CpActorBase>();

        CpMultiHellUpdator newMultiUpdator = new CpMultiHellUpdator(multiHellParam, option, context);
        newMultiUpdator.Start();
        _multiHellUpdators.Add(newMultiUpdator);

        return newMultiUpdator.GetId();
    }
    public void RequestStart(List<CpHellParamListElement> hellParamList, out List<FCpMultiHellUpdatorId> outHellUpdatorIdList)
    {
        outHellUpdatorIdList = new List<FCpMultiHellUpdatorId>(hellParamList.Count);

        foreach (CpHellParamListElement elem in hellParamList)
        {
            FCpMultiHellUpdatorId id = RequestStart(elem.Setting.MultiHellParam, elem.Option);
            outHellUpdatorIdList.Add(id);
        }
    }


    public UnityEvent<FCpMultiHellUpdatorId> OnHellFinished => _onHellFinished;

    List<CpMultiHellUpdator> _multiHellUpdators = new List<CpMultiHellUpdator>();
    UnityEvent<FCpMultiHellUpdatorId> _onHellFinished = new UnityEvent<FCpMultiHellUpdatorId>();
}
