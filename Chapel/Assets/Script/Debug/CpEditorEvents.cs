using UnityEditor;
using UnityEngine;
#if CP_EDITOR
// コンパイルされたときに自動的にロードされます（エディタ限定)
[InitializeOnLoadAttribute]
public static class CpEditorEvents
{
    static CpEditorEvents()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.ExitingPlayMode:
                CpDebugParam.Reset();
                break;
        }
    }

}

#endif