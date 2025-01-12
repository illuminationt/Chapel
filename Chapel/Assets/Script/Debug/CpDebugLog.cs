using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CpDebug
{
    //const string ConditionalDefine = "DEBUG_LOG_ON";
    //[System.Diagnostics.Conditional(ConditionalDefine)]
    public static void Log(object Message) => UnityEngine.Debug.Log(Message);
    public static void LogWarning(object Message) => UnityEngine.Debug.LogWarning(Message);
    public static void LogError(object Message) => UnityEngine.Debug.LogError(Message);
    public static string LogErrorRetStr(object Message)
    {
        UnityEngine.Debug.LogError(Message);
        return "ERROR";
    }
    public static void LogError() => LogError("なにかエラー起きてます");
    public static void LogVerbose(object Message)
    {

    }
    public static void LogVerbose()
    {
        LogVerbose("なにかVerboseなエラーが起きているかも");
    }

    public static bool GetKey(KeyCode Key) => Input.GetKey(Key);
    public static bool GetKeyDown(KeyCode Key) => Input.GetKeyDown(Key);
    public static bool GetKeyUp(KeyCode Key) => Input.GetKeyUp(Key);
}