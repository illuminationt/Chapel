using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 向く方向を強制させる機能を提供するインターフェース
public interface ICpLockonControlInterface
{
    public bool GetLockonDirection(ref Vector2 direction);
}


public class CpPlayerLockonController : ICpLockonControlInterface
{
    enum ECpState
    {
        None,
        Enable,
        Disable,
    }
    public static CpPlayerLockonController Create(Transform ownerTransform, ICpActorForwardInterface ownerForward)
    {
        CpPlayerLockonController instance = new CpPlayerLockonController();
        instance.Initialize(ownerTransform, ownerForward);
        return instance;
    }
    void Initialize(Transform ownerTransform, ICpActorForwardInterface ownerForward)
    {
        _currentState = ECpState.Disable;
        _ownerTransform = ownerTransform;
        _ownerForward = ownerForward;
    }

    public bool GetLockonDirection(ref Vector2 outLockonDir)
    {
        switch (_currentState)
        {
            case ECpState.Enable:
                {
                    if (_currentTarget == null || _currentTarget.GetLockonTargetTransform() == null)
                    {
                        SetState(ECpState.Disable);
                        return false;
                    }
                    Vector2 selfPosition = _ownerTransform.position;
                    Vector2 targetPosition = _currentTarget.GetLockonTargetTransform().position;

                    float faceYaw = SltMath.CalcFaceRotationZ(selfPosition, targetPosition);
                    outLockonDir = SltMath.ToVector(faceYaw);
                    return true;
                }
            case ECpState.Disable:
                return false;

            default:
                Assert.IsTrue(false);
                return false;
        }
    }

    public void RequestStart()
    {
        bool bResult = RequestStartInternal();
        if (bResult)
        {
            SetState(ECpState.Enable);
        }
        else
        {
            SetState(ECpState.Disable);
            _currentTarget = null;
        }
    }

    bool RequestStartInternal()
    {
        ICpLockonTarget target = SelectLockonTarget();
        if (target != null)
        {
            _currentTarget = target;
            return true;
        }
        return false;
    }

    public void RequestStop()
    {
        SetState(ECpState.Disable);
    }

    public void Toggle()
    {
        switch (_currentState)
        {
            case ECpState.Enable:
                RequestStop();
                break;
            case ECpState.Disable:
                RequestStart();
                break;
            default:
                Assert.IsTrue(false);
                break;
        }
    }

    ICpLockonTarget SelectLockonTarget()
    {
        Vector2 selfPos = _ownerTransform.position;
        float minDistSq = float.MaxValue;
        ICpLockonTarget candidate = null;

        CpInterfaceContainer.Get().ForEach<ICpLockonTarget>((ICpLockonTarget target) =>
        {
            Vector2 targetPos = target.GetLockonTargetTransform().position;
            float distSq = selfPos.GetSquaredDistanceTo(targetPos);
            if (distSq < minDistSq)
            {
                candidate = target;
                distSq = minDistSq;
            }
        });

        return candidate;
    }


    void SetState(ECpState newState)
    {
        if (newState == _currentState)
        {
            return;
        }
        _currentState = newState;
    }

    List<ICpLockonTarget> GetLockonTargetList()
    {
        MonoBehaviour[] behaviors = MonoBehaviour.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        List<ICpLockonTarget> targets = new List<ICpLockonTarget>();
        foreach (MonoBehaviour behavior in behaviors)
        {
            ICpLockonTarget target = behavior as ICpLockonTarget;
            if (target != null)
            {
                targets.Add(target);
            }
        }

        return targets;
    }

    Transform _ownerTransform = null;
    ICpActorForwardInterface _ownerForward = null;

    ICpLockonTarget _currentTarget = null;
    ECpState _currentState = ECpState.None;
}
