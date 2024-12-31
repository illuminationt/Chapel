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
    }

    public CpPlayer Player
    {
        get
        {
            if (_player == null)
            {
                _player = FindObjectOfType<CpPlayer>();
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
                _objectPool = CpObjectPool.Create();
            }
            return _objectPool;
        }
    }

    public CpGameSettings GameSettings => _gameSettings;
    CpGameSettings _gameSettings;
}
