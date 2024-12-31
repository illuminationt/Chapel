using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Assertions;

public abstract class SltGameObjectCustomEventBase<TArgs> : GameObjectEventUnit<TArgs>
{
    [DoNotSerialize]// No need to serialize ports.
    public ValueOutput result { get; protected set; }// The event output 


    protected override string hookName => GetEventName();

    public override Type MessageListenerType => null;

    protected abstract string GetEventName();

    protected override void Definition()
    {
        base.Definition();
        DefinitionInternal();
    }
    protected virtual void DefinitionInternal()
    {
        // イベントから受け取る引数を記述します。
        // 引数が存在しない場合は空の関数として定義してください。
        // 例：
        // result = ValueOutput<int>(nameof(result));
        throw new System.NotImplementedException();
    }


    protected override bool ShouldTrigger(Flow flow, TArgs args)
    {
        GameObject TargetObject = flow.GetValue<GameObject>(target);
        return flow.stack.self == TargetObject;
    }

    protected override void AssignArguments(Flow flow, TArgs data)
    {
        flow.SetValue(result, data);
    }

    //public static void Trigger(GameObject Target, TArgs Arg)
    //{
    //    EventBus.Trigger(GetEventName(), Target, Arg);
    //}
}
