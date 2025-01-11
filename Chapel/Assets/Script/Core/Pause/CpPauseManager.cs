using UnityEngine;

public enum ECpPauseType
{
    NoPlayable,
}

public class CpPauseManager
{
    public static CpPauseManager Create()
    {
        CpPauseManager manager = new CpPauseManager();
        return manager;
    }

    public static CpPauseManager Get() => CpGameManager.Instance.PauseManager;
}
