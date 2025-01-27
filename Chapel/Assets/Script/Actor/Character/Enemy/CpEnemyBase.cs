using ImGuiNET;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public struct FCpEnemyInitializeParam
{
    public CpEnemySpawner OwnerSpawner;
    public CpEnemySpawnLocationParam LocationParam;
    public CpSpawnedEnemySpecificParam EnemySpecificParam;
}

[RequireComponent(typeof(CpTaskComponent))]
public class CpEnemyBase : CpCharacterBase, ICpLockonTarget
{
    public static UnityEvent<CpEnemyBase> OnEnemyDead = new UnityEvent<CpEnemyBase>();

    [SerializeField] CpItemDropParamScriptableObject _itemDropSettings = null;
    [SerializeField] CpItemDropParam _itemDropParam = null;
    public void InitializeEnemy(in FCpEnemyInitializeParam initParam)
    {
        _ownerSpawner = initParam.OwnerSpawner;
        // 座標設定
        Vector2 position = initParam.LocationParam.GetSpawnLocation();
        transform.position = position;

        // スポーンしたエネミーごとの特有パラメータ設定
        foreach (CpEnemySpecificBehaviorBase behavior in initParam.EnemySpecificParam.SpecificBehaviorList)
        {
            behavior.Apply(this);
        }

        gameObject.SetActive(true);

        SltDelay.Delay(this, initParam.EnemySpecificParam.StartDelay, () =>
        {
            // 行動開始
            CpTaskComponent taskComp = GetComponent<CpTaskComponent>();
            taskComp.StartStateMachine();
        });
    }

    // CpActorBase interface
    protected override void Awake()
    {
        base.Awake();
        ICpContainable.OnAwakeContainable<ICpLockonTarget>(this);
    }

    protected override void OnDestroy()
    {
        ICpContainable.OnDestroyContainable<ICpLockonTarget>(this);
        base.OnDestroy();
    }

    public override ECpMoverUpdateType GetMoverUpdateType() { return ECpMoverUpdateType.UpdateFunction; }

    // end of CpActorBase interface

    // CpCharacterBase interface

    protected override void OnDead()
    {
        OnEnemyDead.Invoke(this);

        if (ItemDropParam.IsValidParam())
        {
            CpItemDropper.Create(transform, ItemDropParam);
        }

        Destroy(gameObject);
    }

    // end of CpCharacterBase

    public override float GetForwardDegree()
    {
        return _transform.eulerAngles.z;
    }

    // ICpAttackSendable
    public override ECpAttackSenderGroup GetAttackSenderGroup()
    {
        return ECpAttackSenderGroup.Enemy;
    }
    public override FCpAttackSendParam CreateAttackSendParam()
    {
        FCpAttackSendParam sendParam;
        sendParam.Attack = 1f;
        return sendParam;
    }
    // end of ICpAttackSendable

    // IC
    public override ECpAttackReceiverGroup GetAttackReceiverGroup()
    {
        return ECpAttackReceiverGroup.Enemy;
    }
    public override void OnReceiveAttack(in FCpAttackSendParam attackSendParam)
    {
        base.OnReceiveAttack(attackSendParam);
    }

    // 

    // ICpLockonTarget
    public Transform GetLockonTargetTransform() { return _transform; }

    // end of ICpLockonTarget
    public void SetStateGraph(StateGraphAsset stateGraphAsset)
    {
        StateMachine stateMachineComp = GetComponent<StateMachine>();
        Assert.IsTrue(stateMachineComp != null, $"{name}にStateMachineスクリプトがアタッチされていません");
        stateMachineComp.nest.macro = stateGraphAsset;
    }

    CpItemDropParam ItemDropParam
    {
        get
        {
            return _itemDropSettings ? _itemDropSettings.Param : _itemDropParam;
        }
    }

    // 
    CpEnemySpawner _ownerSpawner = null;

#if CP_DEBUG
    public override void DrawImGui()
    {
        base.DrawImGui();

        CpTaskComponent taskComp = GetComponent<CpTaskComponent>();
        if (ImGui.TreeNode("Task Component"))
        {
            taskComp.DrawImGui();
            ImGui.TreePop();
        }

        if (ImGui.TreeNode("Misc"))
        {
            CpPlayer player = CpPlayer.Get();
            Vector2 playerPos = player.transform.position;

            float distToPlayer = playerPos.GetDistanceTo(transform.position);
            string distToPlayerStr = $"DistanceToPlayer = {distToPlayer}";
            ImGui.Text(distToPlayerStr);

            ImGui.TreePop();
        }
    }
#endif
}
