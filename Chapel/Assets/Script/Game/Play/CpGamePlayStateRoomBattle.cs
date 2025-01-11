using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CpGamePlayStateRoomBattle : CpGamePlayStateBase
{
    public override ECpGamePlayState GetGamePlayState()
    {
        return ECpGamePlayState.RoomBattle;
    }
    public override List<ECpGamePlayState> GetCanStackStates()
    {
        return new List<ECpGamePlayState> { ECpGamePlayState.RoomExplore };
    }

    public void Setup(CpRoomProxy roomProxy, CpRoomUsableParamBattle roomParamBattle)
    {
        // バトルステートのSetup呼ばれてない！
        _roomProxy = roomProxy;
        _roomParamBattle = roomParamBattle;

    }

    protected override void OnStartInternal()
    {
        CpEnemySpawnParam enemySpawnParam = _roomParamBattle.EnemySpawnParam;

        CpEnemySpawnerManager enemySpawnerManager = CpEnemySpawnerManager.Get();
        _enemySpawner = enemySpawnerManager.RequestSpawn(enemySpawnParam);
    }

    protected override void UpdateInternal()
    {
        if (_enemySpawner.IsAllEnemyDead())
        {
            FinishState();
        }
    }

    protected override void OnFinishedInternal()
    {
        _roomProxy.OnAllRoomEnemyDestroyed();
    }

    CpRoomProxy _roomProxy = null;
    CpRoomUsableParamBattle _roomParamBattle = null;
    CpEnemySpawner _enemySpawner = null;
}