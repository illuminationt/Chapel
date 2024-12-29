using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Assertions;

// VisualScripting Unitの基底
public abstract class SltUnitBase : Unit
{
    // インプットノード
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput input { get; private set; }

    // アウトプット（デフォルト、ノード入ったらすぐ実行する）
    [DoNotSerialize]
    [PortLabelHidden]// フローの名前を非表示にする
    public ControlOutput outputDefault { get; private set; }
    // アウトプット（タスク成功）
    [DoNotSerialize]
    public ControlOutput outputSucceed { get; private set; }
    // アウトプット（外部から終了させられた）
    [DoNotSerialize]
    public ControlOutput outputExternal { get; private set; }

    // 変数定義

    // アウトプットのDefaultを表示するフラグ;継承先のコンストラクタで設定します
    protected bool bUseDefault = true;
    // アウトプットのOnSucceedを表示する;継承先のコンストラクタで設定します
    protected bool bUseOnSucceed = true;
    // アウトプットのOnExternalを表示する;継承先のコンストラクタで設定します
    protected bool bUseOnFinishExternal = false;

    // タスク生成関数。派生先でオーバーライド必須。
    // Unitとタスクは1対1対応するはず！
    protected abstract SltTaskBase CreateTask();
    protected virtual void InitializeUnitVariables(Flow flow) { }

    protected sealed override void Definition()
    {
        input = ControlInput("", OnEnterUnit_Internal);

        if (bUseDefault)
        {
            outputDefault = ControlOutput("");
        }

        if (bUseOnSucceed)
        {
            outputSucceed = ControlOutput("OnSucceeded");
        }

        if (bUseOnFinishExternal)
        {
            outputExternal = ControlOutput("FinishExternal");
        }

        DefinitionInternal();
    }
    protected virtual void DefinitionInternal() { }

    private ControlOutput OnEnterUnit_Internal(Flow flow)
    {
        GraphReference graphReference = flow.stack.ToReference();
        GameObject Self = flow.stack.self;
        SltTaskComponent ownerComponent = Self.GetComponent<SltTaskComponent>();
        if (ownerComponent == null)
        {
            Assert.IsTrue(false);
            //SltDebug.LogError(Self.name + "'s TaskComponent is null");
            return null;
        }

        // タスクで使用するUnitの変数を初期化
        InitializeUnitVariables(flow);

        // タスク生成＆実行するコンポーネントに登録
        SltTaskBase task = CreateTask();
        task.UnitGuid = guid;
        // 全Unit共通で持つOutputフローを登録
        if (outputDefault != null && outputDefault.hasAnyConnection)
        {
            task.OnTaskStart.AddListener(() => Flow.New(graphReference).Run(outputDefault));
        }

        // Unitから出るために実行するUnityActionにフローの関数をバインド
        if (outputSucceed != null && outputSucceed.hasAnyConnection)
        {
            task.OnFinishAction.AddListener(() => Flow.New(graphReference).Run(outputSucceed));
        }

        // Unityから出るFInishExternalバインド
        if (outputExternal != null && outputExternal.hasAnyConnection)
        {
            task.OnFinishExternal.AddListener(() => Flow.New(graphReference).Run(outputExternal));
        }

        // タスク実行コンポーネントにタスクを登録
        ownerComponent.RegisterTask(task);

        return outputDefault;
    }
}
