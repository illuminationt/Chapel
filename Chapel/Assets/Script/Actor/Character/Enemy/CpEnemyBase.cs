using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct FCpEnemyInitializeParam
{
    public CpEnemySpawner OwnerSpawner;
    public CpEnemySpawnLocationParam LocationParam;
    public CpSpawnedEnemySpecificParam EnemySpecificParam;
}

[RequireComponent(typeof(CpTaskComponent))]
public class CpEnemyBase : CpCharacterBase
{
    public static UnityEvent<CpEnemyBase> OnEnemyDead = new UnityEvent<CpEnemyBase>();

    [SerializeField] CpItemDropParam _itemDropParam = null;
    public void InitializeEnemy(in FCpEnemyInitializeParam initParam)
    {
        _ownerSpawner = initParam.OwnerSpawner;
        // 座標設定
        Vector2 position = initParam.LocationParam.GetSpawnLocation();
        transform.position = position;

        // スポーンしたエネミーごとの特有パラメータ設定

        SltDelay.Delay(this, initParam.EnemySpecificParam.StartDelay, () =>
        {
            // 行動開始
            CpTaskComponent taskComp = GetComponent<CpTaskComponent>();
            taskComp.StartStateMachine();
        });
    }


    // CpCharacterBase interface

    protected override void OnDead()
    {
        OnEnemyDead.Invoke(this);

        if (_itemDropParam.IsValidParam())
        {
            CpItemDropper.Create(transform, _itemDropParam);
        }

        Destroy(gameObject);
    }

    // end of CpCharacterBase

    public override float GetForwardDegree()
    {
        return 0f;
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
    CpEnemySpawner _ownerSpawner = null;
}
