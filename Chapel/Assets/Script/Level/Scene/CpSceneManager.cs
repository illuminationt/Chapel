using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public enum ECpSceneType
{
    None = 0,
    Title = 10,
    Gameplay_Core = 100,
    Gameplay_Dungeon = 110,
}

[System.Serializable]
public class CpSceneParam
{
    public ECpSceneType SceneType;
    public string SceneName;
    public LoadSceneMode LoadSceneMode;
}
public static class CpSceneName
{
    public static string Get(ECpSceneType sceneType)
    {
        CpSceneSettings sceneSettings = CpSceneSettings.Get();
        return sceneSettings.GetSceneName(sceneType);
    }
}

public class CpSceneLoader
{
    MonoBehaviour _monoBehavior = null;
    AsyncOperation asyncOp = null;
    ECpSceneType _sceneType = ECpSceneType.None;

    public static CpSceneLoader RequestLoad(MonoBehaviour ownerMonoBehavior, ECpSceneType sceneType)
    {
        CpSceneLoader newLoader = new CpSceneLoader();
        newLoader.RequestLoadInternal(ownerMonoBehavior, sceneType);
        return newLoader;
    }

    bool RequestLoadInternal(MonoBehaviour ownerMonoBehavior, ECpSceneType sceneType)
    {
        _monoBehavior = ownerMonoBehavior;

        CpSceneSettings sceneSettings = CpSceneSettings.Get();
        LoadSceneMode loadSceneMode = sceneSettings.GetLoadSceneMode(sceneType);

        _monoBehavior.StartCoroutine(RequestLoadImpl(sceneType, loadSceneMode));

        return true;
    }

    public bool IsLoadFinished()
    {
        if (asyncOp == null)
        {
            return false;
        }

        float progress = asyncOp.progress;
        if (SltMath.IsNearlyEqual(progress, 0.9f) || progress > 0.9f)
        {
            return true;
        }

        return false;
    }

    public void ActivateScene()
    {
        Assert.IsTrue(IsLoadFinished());

        asyncOp.allowSceneActivation = true;
    }

    public bool IsMatchSceneType(ECpSceneType sceneType)
    {
        return sceneType == _sceneType;
    }

    IEnumerator RequestLoadImpl(ECpSceneType sceneType, LoadSceneMode loadSceneMode)
    {
        string sceneName = CpSceneName.Get(sceneType);
        LoadSceneParameters parameters = new LoadSceneParameters();
        parameters.localPhysicsMode = LocalPhysicsMode.Physics2D;
        parameters.loadSceneMode = loadSceneMode;
        asyncOp = SceneManager.LoadSceneAsync(sceneName, parameters);
        Assert.IsTrue(asyncOp != null);

        asyncOp.allowSceneActivation = false;

        while (!asyncOp.isDone)
        {
            yield return null;
        }
    }
}

public class CpSceneManager
{
    public static CpSceneManager Create(MonoBehaviour ownerMonoBehavior)
    {
        CpSceneManager obj = new CpSceneManager();
        obj._ownerMonoBehavior = ownerMonoBehavior;
        return obj;
    }
    public static CpSceneManager Get()
    {
        return CpGameManager.Instance.SceneManager;
    }

    public CpSceneLoader RequestLoadScene(ECpSceneType sceneType)
    {
        if (ExistsScene(sceneType))
        {
            // Assert.IsTrue(false, $"{sceneType}はすでに存在します");
            return null;
        }

        if (IsSceneLoadRequested(sceneType))
        {
            //Assert.IsTrue(false, $"{sceneType}は既にロードリクエストを受けています");
            return null;
        }

        CpSceneLoader sceneLoader = CpSceneLoader.RequestLoad(_ownerMonoBehavior, sceneType);
        _sceneLoaders.Add(sceneLoader);
        return sceneLoader;
    }


    public CpSceneLoader RequestUnload(ECpSceneType sceneType)
    {
        return null;
    }

    private void Update()
    {
        for (int index = _sceneLoaders.Count - 1; index >= 0; index--)
        {
            CpSceneLoader sceneLoader = _sceneLoaders[index];
            if (sceneLoader.IsLoadFinished())
            {
                sceneLoader.ActivateScene();
                _sceneLoaders.RemoveAt(index);
            }
        }
    }

    bool ExistsScene(ECpSceneType sceneType)
    {
        CpSceneSettings sceneSettings = CpSceneSettings.Get();
        string sceneName = sceneSettings.GetSceneName(sceneType);

        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (scene.IsValid() && scene.isLoaded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool IsSceneLoadRequested(ECpSceneType sceneType)
    {
        foreach (CpSceneLoader sceneLoader in _sceneLoaders)
        {
            if (sceneLoader.IsMatchSceneType(sceneType))
            {
                return true;
            }
        }
        return false;
    }

    MonoBehaviour _ownerMonoBehavior = null;
    List<CpSceneLoader> _sceneLoaders = new List<CpSceneLoader>();
}
