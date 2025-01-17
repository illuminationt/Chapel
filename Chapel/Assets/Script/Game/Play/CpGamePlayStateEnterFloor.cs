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
        CpSceneManager sceneManager = CpSceneManager.Get();

        CpSceneLoader coreSceneLoader = sceneManager.RequestLoadScene(ECpSceneType.Gameplay_Core);
        if (coreSceneLoader != null)
        {
            _sceneLoaders.Add(coreSceneLoader);
        }

        CpSceneLoader dungeonSceneLoader = sceneManager.RequestLoadScene(_floorMasterSettings.SceneType);
        if (dungeonSceneLoader != null)
        {
            _sceneLoaders.Add(dungeonSceneLoader);
        }
    }

    protected override void UpdateInternal()
    {
        if (IsAllScenesLoaded())
        {
            foreach (var scene in _sceneLoaders)
            {
                scene.ActivateScene();
            }

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
    bool IsAllScenesLoaded()
    {
        for (int index = 0; index < _sceneLoaders.Count; index++)
        {
            if (!_sceneLoaders[index].IsLoadFinished())
            {
                return false;
            }
        }
        return true;
    }

    CpFloorMasterDataScriptableObject _floorMasterSettings = null;
    List<CpSceneLoader> _sceneLoaders = new List<CpSceneLoader>();
}