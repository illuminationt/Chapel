using ImGuiNET;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ECpMoverUpdateResult
{
    NotActive,// 活動中ではない
    Moving,//移動中
    Finished,// ちょうど終わったところ
}

public enum ECpMoveStopReason
{
    MoverFinished,//
    External,
}

public partial class CpMoverManager
{
    public CpMoverManager(CpActorBase actor)
    {
        _ownerActor = actor;
    }

    public void RequestStopAll()
    {
        foreach (CpMoverBase mover in _currentMovers)
        {
            mover.ExternalCancel();
        }
    }

    public void RequestStop(in FCpMoverId id, ECpMoveStopReason reason)
    {
        CpMoverBase mover = FindMover(id);
        mover?.ExternalCancel();
    }

    public ECpMoverUpdateResult Update(float currentYaw)
    {
        if (_currentMovers.Count == 0)
        {
            _latestDeltaMove = Vector2.zero;
            _latestDeltaYaw = 0f;
            return ECpMoverUpdateResult.NotActive;
        }

        // 更新
        for (int index = 0; index < _currentMovers.Count; index++)
        {
            _currentMovers[index].Update();
        }

        UpdateLatestDeltaMove();
        UpdateLatestDeltaYaw(currentYaw);

        // 終了したMoverを削除
        for (int index = _currentMovers.Count - 1; index >= 0; index--)
        {
            if (_currentMovers[index].IsFinished())
            {
                FCpMoverId finishedMoverId = _currentMovers[index].GetId();
                _currentMovers[index] = null;
                _currentMovers.RemoveAt(index);

                OnMoveFinished.Invoke(finishedMoverId);
            }
        }

        if (_currentMovers.Count > 0)
        {
            return ECpMoverUpdateResult.Moving;
        }
        else
        {
            return ECpMoverUpdateResult.NotActive;
        }
    }

    // パラメータがMonobehaviorに適用されたときのコールバック

    public void OnMoverValueApplied()
    {
        foreach (CpMoverBase mover in _currentMovers)
        {
            mover.OnMoverValueApplied();
        }
    }
    void UpdateLatestDeltaMove()
    {
        Vector2 retDeltaMove = Vector2.zero;
        for (int index = 0; index < _currentMovers.Count; index++)
        {
            retDeltaMove += _currentMovers[index].GetDeltaMove();
        }

        _latestDeltaMove = retDeltaMove;
    }
    void UpdateLatestDeltaYaw(float currentYaw)
    {
        float totalDeltaYaw = 0f;
        foreach (CpMoverBase mover in _currentMovers)
        {
            totalDeltaYaw += mover.GetDeltaYaw(currentYaw);
        }

        _latestDeltaYaw = totalDeltaYaw;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        for (int index = 0; index < _currentMovers.Count; index++)
        {
            _currentMovers[index].OnCollisionEnterHit2D(collision);
        }
    }

    FCpMoverContext CreateContext()
    {
        FCpMoverContext context;
        context.OwnerMoverManager = this;
        context.OwnerActor = _ownerActor;
        context.InitialOwnerPosition = _ownerActor.transform.position;
        context.InitialVelocity = GetVelocity();

        ICpActorForwardInterface forwardInterface = _ownerActor;
        context.InitialOwnerDegree = forwardInterface.GetForwardDegree();
        return context;
    }

    public Vector2 GetDeltaMove()
    {
        return _latestDeltaMove;
    }
    public float GetDeltaRotZ()
    {
        return _latestDeltaYaw;
    }
    public Vector2 GetVelocity()
    {
        return _latestDeltaMove / Time.smoothDeltaTime;
    }

    CpMoverBase FindMover(in FCpMoverId id)
    {
        foreach (CpMoverBase mover in _currentMovers)
        {
            if (mover.GetId().Equals(id))
            {
                return mover;
            }
        }
        return null;
    }

    bool ExistsActiveMover(System.Type moverType)
    {
        foreach (CpMoverBase mover in _currentMovers)
        {
            if (mover.GetType() == moverType)
            {
                if (!mover.IsFinished())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Reset()
    {
        _latestDeltaMove = Vector2.zero;
        _currentMovers.Clear();
        _latestMoverUpdateResult = ECpMoverUpdateResult.NotActive;
        _onMoveFinished.RemoveAllListeners();
    }

    public UnityEvent<FCpMoverId> OnMoveFinished => _onMoveFinished;

    Vector2 _latestDeltaMove = Vector2.zero;
    float _latestDeltaYaw = 0f;

    List<CpMoverBase> _currentMovers = new List<CpMoverBase>();
    ECpMoverUpdateResult _latestMoverUpdateResult = ECpMoverUpdateResult.NotActive;
    CpActorBase _ownerActor = null;

    UnityEvent<FCpMoverId> _onMoveFinished = new UnityEvent<FCpMoverId>();

#if CP_DEBUG
    public void DrawImGui()
    {
        if (ImGui.TreeNode("Movers"))
        {
            foreach (CpMoverBase mover in _currentMovers)
            {
                string typeStr = mover.GetType().ToString();

                string treeTitle = $"[{mover.GetId().ToString()}]{typeStr}";
                if (ImGui.TreeNode(treeTitle))
                {
                    mover.DrawImGui();
                    ImGui.TreePop();
                }
            }
            ImGui.TreePop();
        }
    }

    public int DebugGetMoverCount() { return _currentMovers.Count; }

#endif
}
