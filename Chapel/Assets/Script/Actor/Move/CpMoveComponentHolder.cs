using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using static Unity.Burst.Intrinsics.X86.Avx;
public class CpMoveComponentHolder : IDisposable
{
    public static CpMoveComponentHolder Get()
    {
        return CpGameManager.Instance.MoveComponentHolder;
    }
    public CpMoveComponentHolder()
    {
        _updateList = new List<CpMoveComponent>(100);
        _fixedUpdateList = new List<CpMoveComponent>(300);
        CpGameManager.Instance.OnUpdateAsObservaable.Subscribe(Update);
        CpGameManager.Instance.OnFixedUpdateAsObservabale.Subscribe(FixedUpdate);
    }

    void Update(Unit _)
    {
        Update(_updateList);
    }
    void FixedUpdate(Unit _)
    {
        Update(_fixedUpdateList);
    }
    public void Register(CpMoveComponent comp)
    {
        ECpMoverUpdateType updateType = comp.GetMoverUpdateType();
        List<CpMoveComponent> list = GetList(updateType);
        list.Add(comp);
    }
    public void Unregister(CpMoveComponent comp)
    {
        ECpMoverUpdateType updateType = comp.GetMoverUpdateType();
        List<CpMoveComponent> list = GetList(updateType);
        list.RemoveSwapBack(comp);
    }

    public void Dispose()
    {
        _updateList.Clear();
        _fixedUpdateList.Clear();
    }


    [BurstCompile]
    void Update(List<CpMoveComponent> list)
    {
        list.RemoveAll((CpMoveComponent comp) =>
        {
            if (comp == null)
            {
                return true;
            }
            return false;
        });
        foreach (CpMoveComponent comp in list)
        {
            if (comp.enabled)
            {
                comp.Execute();
            }
        }
    }
    List<CpMoveComponent> GetList(ECpMoverUpdateType updateType)
    {
        switch (updateType)
        {
            case ECpMoverUpdateType.UpdateFunction:
                return _updateList;
            case ECpMoverUpdateType.FixedUpdateFunction:
                return _fixedUpdateList;
            default:
                Assert.IsTrue(false, $"updateType={updateType}");
                return null;
        }
    }

    List<CpMoveComponent> _fixedUpdateList;
    List<CpMoveComponent> _updateList;
}
