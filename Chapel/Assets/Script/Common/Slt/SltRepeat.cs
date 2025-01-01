using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public static class SltRepeat
{
    public static void Repeat(
        MonoBehaviour Obj, float Interval, int RepeatCount, float StartDelay, UnityAction Action, bool bScaleTime = true)
    {
        if (StartDelay == 0f && RepeatCount == 1)
        {
            Action.Invoke();
        }
        else
        {
            Obj.StartCoroutine(RepeatCoroutine(Obj, Interval, RepeatCount, StartDelay, Action, bScaleTime));
        }
    }

    static IEnumerator RepeatCoroutine(
        MonoBehaviour Obj,
        float Interval, int RepeatCount, float StartDelay,
        UnityAction Action, bool bScaleTime = true)
    {
        // ‚P‰ñ–Ú‚ÌAction‚Ü‚Å‚Ì’x‰„
        if (StartDelay != 0f)
        {
            yield return SltDelay.DelayCoroutine(StartDelay, Action, bScaleTime);
        }

        // Repeat
        for (int Count = 0; Count < RepeatCount; Count++)
        {
            yield return SltDelay.DelayCoroutine(Interval, Action, bScaleTime);
        }

    }
}