using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class CpGameManager : SingletonMonoBehaviour<CpGameManager>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        GameObject Obj = new GameObject("CpGameManager", typeof(CpGameManager));
        DontDestroyOnLoad(Obj);
        Obj.SetActive(true);

        CpGameManager gameManager = Obj.GetComponent<CpGameManager>();
        gameManager.enabled = true;

        gameManager.InitializeInternal();
    }
    void InitializeInternal()
    {
        var op = Addressables.LoadAssetAsync<CpGameSettings>("CpGameSettings");
        _gameSettings = op.WaitForCompletion();
        _enemySpawnerManager = CpEnemySpawnerManager.Create();
        _dungeonManager = CpDungeonManager.Create();
        _gameFlowManager = CpGameFlowManager.Create();
        _gamePlayManager = CpGamePlayManager.Create();
        _pauseManager = CpPauseManager.Create();
    }

    private void Update()
    {
        _gamePlayManager.Update();
    }

    public CpPlayer Player
    {
        get
        {
            if (_player == null)
            {
                _player = FindFirstObjectByType<CpPlayer>();
            }
            return _player;
        }
    }
    CpPlayer _player = null;

    CpInputManager _inputManager = null;
    public CpInputManager InputManager
    {
        get
        {
            if (_inputManager == null)
            {

                _inputManager = CpInputManager.Create();
            }
            return _inputManager;
        }
    }

    CpObjectPool _objectPool = null;
    public CpObjectPool ObjectPool
    {
        get
        {
            if (_objectPool == null)
            {
                _objectPool = CpObjectPool.CreateObjectPool();
            }
            return _objectPool;
        }
    }
    public CpEnemySpawnerManager EnemySpawnerManager => _enemySpawnerManager;
    CpEnemySpawnerManager _enemySpawnerManager = null;
    public CpDungeonManager DungeonManager => _dungeonManager;
    CpDungeonManager _dungeonManager = null;

    public CpGameFlowManager GameFlowManager => _gameFlowManager;
    CpGameFlowManager _gameFlowManager = null;

    public CpGamePlayManager GamePlayManager => _gamePlayManager;
    CpGamePlayManager _gamePlayManager = null;

    public CpPauseManager PauseManager => _pauseManager;
    CpPauseManager _pauseManager = null;
    public CpGameSettings GameSettings => _gameSettings;
    CpGameSettings _gameSettings;

#if DEBUG
    //void ShowImGui(bool bShow)
    //{
    //    if (_imGuiRootInstance == null)
    //    {
    //        GameObject prefab = (GameObject)Resources.Load("P_ImGui");
    //        GameObject instance = Instantiate(prefab);
    //        _imGuiRootInstance = instance.GetComponent<CpImGui>();
    //    }

    //    _imGuiRootInstance.SetActive(bShow);
    //}

    //CpImGui _imGuiRootInstance = null;
#endif
}
