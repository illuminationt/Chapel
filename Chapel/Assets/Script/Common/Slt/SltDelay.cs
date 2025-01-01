using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class SltDelay
{
    // à¯êîÇ»Çµ
    public static Coroutine Delay(MonoBehaviour Obj, float Delay, UnityAction Action, bool bScaleTime = true)
    {
        if (Action == null)
        {
            Debug.LogError("Action == null");
            return null;
        }
        if (Delay == 0f)
        {
            Action.Invoke();
            return null;
        }

        return Obj.StartCoroutine(DelayCoroutine(Delay, Action, bScaleTime));
    }

    public static Coroutine DelayNextFrame(MonoBehaviour Obj, UnityAction Action)
    {
        return Delay(Obj, 0.001f, Action);
    }

    public static IEnumerator DelayCoroutine(float Delay, UnityAction Action, bool bScaleTime)
    {
        if (bScaleTime)
        {
            yield return new WaitForSeconds(Delay);
        }
        else
        {
            yield return new WaitForSecondsRealtime(Delay);
        }
        Action.Invoke();
    }

    // à¯êîÇPå¬
    public static Coroutine Delay<T>(MonoBehaviour Obj, float Delay, UnityAction<T> Action, T Value, bool bScaleTime = true)
    {
        if (Delay == 0f)
        {
            Action.Invoke(Value);
            return null;
        }

        return Obj.StartCoroutine(DelayCoroutine(Delay, bScaleTime, Action, Value));
    }
    public static Coroutine Delay<T>(GameObject Obj, float DelaySecond, UnityAction<T> Action, T Value, bool bScaleTime = true)
    {
        MonoBehaviour Mono = Obj.GetComponent<MonoBehaviour>();
        return Delay(Mono, DelaySecond, Action, Value, bScaleTime);
    }
    public static IEnumerator DelayCoroutine<T>(float Delay, bool bScaleTime, UnityAction<T> Action, T Value)
    {
        if (bScaleTime)
        {
            yield return new WaitForSeconds(Delay);
        }
        else
        {
            yield return new WaitForSecondsRealtime(Delay);
        }
        Action.Invoke(Value);
    }

    public static Coroutine Delay<T0, T1>(MonoBehaviour Obj, float Delay, UnityAction<T0, T1> Action, T0 Value0, T1 Value1)
    {
        if (Delay == 0f)
        {
            Action(Value0, Value1);
            return null;
        }
        return Obj.StartCoroutine(DelayCoroutine(Delay, Action, Value0, Value1));
    }

    static IEnumerator DelayCoroutine<T0, T1>(float Delay, UnityAction<T0, T1> Action, T0 Value0, T1 Value1)
    {
        yield return new WaitForSeconds(Delay);
        Action.Invoke(Value0, Value1);
    }
}