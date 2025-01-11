using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CpGamePlayStateEnterFloor : CpGamePlayStateBase
{
    public override ECpGamePlayState GetGamePlayState()
    {
        return ECpGamePlayState.EnterDungeon;
    }
    public override List<ECpGamePlayState> GetCanStackStates()
    {
        return new List<ECpGamePlayState> { ECpGamePlayState.Root };
    }

    public void Setup(CpFloorMasterDataScriptableObject dungeonStructureParam)
    {
        _floorMasterSettings = dungeonStructureParam;
    }

    protected override void OnStartInternal()
    {
        SceneManager.LoadScene("L_DungeonA");
    }

    protected override void UpdateInternal()
    {
        _timer += CpTime.DeltaTime;
        if (_timer > 1.3f && SceneManager.GetActiveScene().name == "L_DungeonA")
        {
            OwnerGamePlayManager.RequestStartRoomExplore();
            FinishState();
        }
    }

    protected override void OnFinishedInternal()
    {
        CpDungeonManager dungeonManager = CpDungeonManager.Get();
        dungeonManager.InitializeDungeon(_floorMasterSettings);

        dungeonManager.LandPlayer();
    }

    CpFloorMasterDataScriptableObject _floorMasterSettings = null;
    float _timer = 0f;
}