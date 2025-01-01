using Oddworm.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CpPilotComponent))]
[RequireComponent(typeof(CpShootComponent))]
public class CpPlayer : CpCharacterBase
{
    Transform _transform = null;
    CpPilotComponent _pilotComponent = null;
    CpShootComponent _shootComponent = null;
    CpDebugComponent _debugComponent = null;
    CpPlayerForwardCalculator _forwardCalculator = null;

    public CpHellParamScriptableObject HellParamScriptableObject = null;
    public CpEnemyBase EnemyPrefab = null;
    public CpEnemySpawnParam EnemySpawnParam = null;
    private void Start()
    {
        _transform = GetComponent<Transform>();
        _pilotComponent = GetComponent<CpPilotComponent>();
        _shootComponent = GetComponent<CpShootComponent>();
        _forwardCalculator = new CpPlayerForwardCalculator(_transform);
    }

    protected override void Update()
    {
        base.Update();
        _pilotComponent.execute();
        _forwardCalculator.execute();
        debugDrawDirection();

        updateShoot();

        if (Input.GetKeyDown(KeyCode.M))
        {
            CpTaskComponent taskComp = GetComponent<CpTaskComponent>();
            taskComp.StartStateMachine();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            CpHellComponent hellComp = GetComponent<CpHellComponent>();
            hellComp.RequestStart(HellParamScriptableObject.MultiHellParam);
        }

        //_timer += CpTime.DeltaTime;
        //if (_timer > 1f && count < 11111111)
        //{
        //    _timer = 0f;
        //    count++;
        //    CpEnemyBase newEnemy = Instantiate(EnemyPrefab); ;
        //    newEnemy.transform.position = new Vector2(0f, 70f);
        //}

        if (Input.GetKeyDown(KeyCode.R))
        {
            CpEnemySpawnerManager enemySpanerManager = CpEnemySpawnerManager.Get();
            enemySpanerManager.RequestSpawn(EnemySpawnParam);
        }

        Vector2 Screenpos = CpUtil.GetScreenPositionFromWorldPositoni(transform.position);
        CpDebug.Log("Player Screen Poss = " + Screenpos);

        if (Input.GetKeyDown(KeyCode.K))
        {
            CpEnemyShot[] shots = FindObjectsByType<CpEnemyShot>(FindObjectsSortMode.None);
            CpDebug.LogError("EnemyShot Num:" + shots.Count());
            for (int i = shots.Count() - 1; i >= 0; i--)
            {
                Destroy(shots[i].gameObject);
            }
            foreach (CpEnemyShot shot in shots)
            {
                Destroy(shot.gameObject);
            }
        }
    }
    float _timer = 0f;
    int count = 0;

    // ICpActorForwardInterface
    public override float GetForwardDegree()
    {
        return ForwardCalculator.GetForwardDegree();
    }
    // end of ICpActorForwardInterface

    // ICpAttackSendable
    public override ECpAttackSenderGroup GetAttackSenderGroup()
    {
        return ECpAttackSenderGroup.Player;
    }
    public override FCpAttackSendParam CreateAttackSendParam()
    {
        FCpAttackSendParam sendParam;
        sendParam.Attack = 1f;
        return sendParam;
    }
    // end of ICpAttackSendable

    // ICpAttackReceivable
    public override ECpAttackReceiverGroup GetAttackReceiverGroup()
    {
        return ECpAttackReceiverGroup.Player;
    }
    public override void OnReceiveAttack(in FCpAttackSendParam attackSendParam)
    {
        CpDebug.LogError("���@���_���[�W�󂯂�");
    }
    // end of ICpAttackReceivable

    CpPlayerForwardCalculator ForwardCalculator
    {
        get
        {
            if (_forwardCalculator == null)
            {
                _forwardCalculator = new CpPlayerForwardCalculator(transform);
            }
            return _forwardCalculator;
        }
    }

    void updateShoot()
    {
        FCpShootControlParam shootControlParam = new FCpShootControlParam();
        shootControlParam.origin = _transform.position;
        shootControlParam.forward = _forwardCalculator.GetForwardVector();
        _shootComponent.execute(shootControlParam);
    }

    void debugDrawDirection()
    {
        Vector2 dir = _forwardCalculator.GetForwardVector();
        Vector2 start = _transform.position;
        Vector2 end = start + dir * 22f;
    }

}
