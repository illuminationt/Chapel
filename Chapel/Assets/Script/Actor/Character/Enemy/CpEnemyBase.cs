using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FCpEnemyInitializeParam
{
    public CpEnemySpawnLocationParam LocationParam;
    public CpSpawnedEnemySpecificParam EnemySpecificParam;
}

[RequireComponent(typeof(CpTaskComponent))]
public class CpEnemyBase : CpCharacterBase
{
    public void Start()
    {

    }

    public void InitializeEnemy(in FCpEnemyInitializeParam initParam)
    {
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
}
