using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ランダム移動する方向
public enum ECpMoveRandomAxisType
{
    None = 0,
    X = 1,
    Y = 2,
    XY = 3,
}

// ランダム移動を繰り返し実行する際の、中心座標の更新方法
public enum ECpUpdateCenterType
{
    None = 0,// 不正値
    NoUpdate = 1, // 初期座標をずっと中心座標として使い続ける
    X = 2,// X座標だけ更新する
    Y = 3,
    XY = 4,
}

[Inspectable]
[System.Serializable]
[IncludeInSettings(true)]
public struct FCpMoveParamRandom
{
    [Inspectable] public float Radius;
    [Inspectable] public float Speed;
    [Inspectable] public Ease EasingType;
    [Inspectable] public Vector2 WaitTimeInterval;
    [Inspectable] public ECpMoveRandomAxisType MoveAxisType;
    [Inspectable] public ECpUpdateCenterType UpdateCenterType;
}

public class CpTaskMoveRandom : CpTaskBase
{
    public CpTaskMoveRandom(in FCpMoveParamRandom param)
    {
        _param = param;
    }

    public override ECpTaskType GetTaskType()
    {
        return ECpTaskType.Move;
    }
    protected override void OnStartInternal()
    {
        _currentCenterWorldPosition = Owner.transform.position;

        CpMoveComponent moveComp = Owner.GetComponent<CpMoveComponent>();
        moveComp.OnMoveFinished.AddListener(OnMoveFinished);

        StartMove();
    }

    protected override void UpdateInternal(float DeltaTime)
    {
    }

    protected override void OnFinishInternal(ESltEndTaskReason endReason)
    {
        CpMoveComponent moveComp = Owner.GetComponent<CpMoveComponent>();
        moveComp.OnMoveFinished.RemoveListener(OnMoveFinished);

        moveComp.RequestStop(_currentMoverId, ECpMoveStopReason.MoverFinished);
    }

    void StartMove()
    {
        CpMoveComponent moveComp = Owner.GetComponent<CpMoveComponent>();
        FCpMoveParamTween paramTween = CreateMoveParamTween();
        _currentMoverId = moveComp.RequestStart(paramTween);
    }

    void OnMoveFinished(FCpMoverId id)
    {
        if (!_currentMoverId.Equals(id))
        {
            return;
        }
        Vector2 currentPosition = Owner.transform.position;
        if (_currentMoverId.Equals(id))
        {
            switch (_param.UpdateCenterType)
            {
                case ECpUpdateCenterType.NoUpdate:
                    // 更新しない
                    break;
                case ECpUpdateCenterType.X:
                    _currentCenterWorldPosition.x = currentPosition.x;
                    break;
                case ECpUpdateCenterType.Y:
                    _currentCenterWorldPosition.y = currentPosition.y;
                    break;
                case ECpUpdateCenterType.XY:
                    _currentCenterWorldPosition = currentPosition;
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }

            float startDelay = _param.WaitTimeInterval.GetRandom();
            if (startDelay <= 0f)
            {
                StartMove();
            }
            else
            {
                SltDelay.Delay(OwnerActor, startDelay, StartMove);
            }
        }
    }

    FCpMoveParamTween CreateMoveParamTween()
    {
        Vector2 vectorValue = _currentCenterWorldPosition + CalcOffsetFromCenter();
        Vector2 currentPosition = Owner.transform.position;

        float deltaMove = currentPosition.GetDistanceTo(vectorValue);

        float duration = deltaMove / _param.Speed;

        return new FCpMoveParamTween(vectorValue, duration, ECpVectorType.AbsoluteWorldPosition, _param.EasingType);
    }

    Vector2 CalcOffsetFromCenter()
    {
        Vector2 retVector = Vector2.zero;
        switch (_param.MoveAxisType)
        {
            case ECpMoveRandomAxisType.X:
                retVector.x = _param.Radius * CpRandom.Range(-1f, 1f);
                break;
            case ECpMoveRandomAxisType.Y:
                retVector.y = _param.Radius * CpRandom.Range(-1f, 1f);
                break;
            case ECpMoveRandomAxisType.XY:
                retVector = _param.Radius * UnityEngine.Random.insideUnitCircle;
                break;
            default:
                Assert.IsTrue(false, $"{Owner.name}が不正なMoveAxisTypeを使用しています");
                break;
        }
        return retVector;
    }

    FCpMoveParamRandom _param;
    FCpMoverId _currentMoverId;

    Vector2 _currentCenterWorldPosition;
}

[UnitTitle("CpTaskMoveRandom")]
[UnitCategory("Cp/Move")]
[UnitSubtitle("CpTaskMoveRandom")]
public class CpUnit_CpTaskMoveRandom : CpUnitBase
{
    ValueInput inputParam;
    FCpMoveParamRandom param;

    protected override SltTaskBase CreateTask(GameObject ownerObj)
    {
        return new CpTaskMoveRandom(param);
    }

    protected override void InitializeUnitVariables(Flow flow)
    {
        // example: vectorValue = flow.GetValue<Vector2>(inputVectorValue);

        param = flow.GetValue<FCpMoveParamRandom>(inputParam);

    }

    protected override void DefinitionInternal()
    {
        // exapmple: inputVectorValue = ValueInput("VectorValue", Vector2.zero);

        inputParam = ValueInput("Param", new FCpMoveParamRandom());

    }
}