using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class CpTaskAnimation : CpTaskBase
{
    GameObject _gameObject = null;
    string _animName = null;

    Animator _animator = null;
    float _speed = 1f;
    bool _bReverse = false;
    int _animLoopNumber = 1;
    int _currentLoopCounter = 0;

    // 前フレームでのアニメーション再生正規化時間の整数部分
    // 1周した後（NormalizedTime>1f）になると1になり、3周したら3になる
    int _prevAnimTimeIntegerPart = 0;
    public CpTaskAnimation(GameObject gameObject, string animName, float speed, bool bReverse)
    {
        bShouldUpdate = true;
        _gameObject = gameObject;
        _animName = animName;
        _speed = speed;
        Assert.IsTrue(_speed > 0f, $"speedは姓の値を入力してください");
        if (bReverse)
        {
            _speed *= -1f;
        }
    }

    public override ECpTaskType GetTaskType()
    {
        return ECpTaskType.Animation;
    }

    protected override void OnStartInternal()
    {
        if (_gameObject == null)
        {
            _gameObject = Owner;
        }
        _animator = _gameObject.GetComponent<Animator>();
        Assert.IsTrue(_animator != null, $"{_gameObject.name}にAnimatorがアタッチされていません");

        _currentLoopCounter = 0;
        _prevAnimTimeIntegerPart = 0;

        PlayAnimation();
    }

    protected override void UpdateInternal(float DeltaTime)
    {
        if (CheckAnimationFinished())
        {
            EndTask(ESltEndTaskReason.FinishTask);
        }
    }

    protected override void OnFinishInternal(ESltEndTaskReason endReason)
    {

    }

    void PlayAnimation()
    {
        _animator.Play(_animName);
        _animator.speed = _speed;
    }


    bool CheckAnimationFinished()
    {
        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = info.normalizedTime;

        int timeIntegerPart = Mathf.FloorToInt(normalizedTime);
        if (_prevAnimTimeIntegerPart == timeIntegerPart)
        {
            return false;
        }
        else
        {
            _prevAnimTimeIntegerPart = timeIntegerPart;
            _currentLoopCounter++;
            if (_currentLoopCounter >= _animLoopNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

[UnitTitle("CpTaskAnimation")]
[UnitCategory("Cp/Animation")]
[UnitSubtitle("CpTaskAnimation")]
public class CpUnit_CpTaskAnimation : CpUnitBase
{
    ValueInput inputAnimationName;
    ValueInput inputGameObject;
    ValueInput inputSpeed;
    ValueInput inputReverse;

    protected string animationName = null;
    protected GameObject gameObject;
    protected float speed = 1f;
    protected bool bReversed = false;
    protected override SltTaskBase CreateTask(GameObject ownerObj)
    {
        return new CpTaskAnimation(gameObject, animationName, speed, bReversed);
    }

    protected override void InitializeUnitVariables(Flow flow)
    {
        // example: vectorValue = flow.GetValue<Vector2>(inputVectorValue);

        animationName = flow.GetValue<string>(inputAnimationName);
        gameObject = flow.GetValue<GameObject>(inputGameObject);
        speed = flow.GetValue<float>(inputSpeed);
        bReversed = flow.GetValue<bool>(inputReverse);
    }

    protected override void DefinitionInternal()
    {
        // exapmple: inputVectorValue = ValueInput("VectorValue", Vector2.zero);

        inputAnimationName = ValueInput("AnimName", "");
        inputGameObject = ValueInput<GameObject>("Anim Actor", null);
        inputSpeed = ValueInput("Speed", 1f);
        inputReverse = ValueInput("Reverse", false);
    }
}