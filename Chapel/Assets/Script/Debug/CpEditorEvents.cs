using UnityEditor;
using UnityEngine;
#if CP_EDITOR
// �R���p�C�����ꂽ�Ƃ��Ɏ����I�Ƀ��[�h����܂��i�G�f�B�^����)
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