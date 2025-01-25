using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using NUnit.Framework;

public static class SltMath
{
    // IsNearlyånìù
    public static bool IsNearlyEqual(float a, float b, float tolerance = 0.01f)
    {
        return Mathf.Abs(a - b) < tolerance;
    }

    public static bool IsNearlyZero(in Vector2 Vec, float Torelance = 0.01f)
    {
        return IsVectorNearlyEqual(Vec, 0f, Torelance);
    }
    public static bool IsVectorNearlyEqual(in Vector2 Vec, float Size, float SizeTolerance = 0.01f)
    {
        float VecSize = Vec.magnitude;
        return IsNearlyEqual(VecSize, Size, SizeTolerance);
    }

    // äpìxåvéZ
    public static float UnwindDegree(float degree)
    {
        while (degree > 180f)
        {
            degree -= 180f;
        }
        while (degree < -180f) { degree += 180f; }
        return degree;
    }

    public static float CalcFaceRotationZ(in Vector2 from, in Vector2 to)
    {
        Vector2 dir = (to - from).normalized;
        float rotZ = SltMath.ToDegree(dir);
        return rotZ;
    }

    public static float UnwindAngle(float angle)
    {
        while (angle > 180f)
        {
            angle -= 360f;
        }
        while (angle < -180f)
        {
            angle += 360f;
        }
        return angle;
    }

    // CtdFuncLib.GetRotateDeltaÇ©ÇÁà⁄êA
    public static float GetRotateDelta(float from, float to, float equalTorelance)
    {
        // Ç‹Ç∏ê≥ãKâª
        from = UnwindAngle(from);
        to = UnwindAngle(to);
        if (IsNearlyEqual(from, to, equalTorelance))
        {
            // ìØÇ∂ílÇ»ÇÁï˚å¸ÇÕÉ[ÉçÇï‘Ç∑
            return 0f;
        }

        // åvéZÇÃÇΩÇﬂ,ToÇ™ëÂÇ´Ç≠Ç»ÇÈÇÊÇ§Ç…ïœçXâ¡Ç¶ÇÈ
        if (to < from)
        {
            to += 360f;
        }

        if (from + 180f < to)
        {
            // åªç›ÇÃílÇ…180Çâ¡Ç¶ÇƒÇ‡ToÇ…ÇΩÇ«ÇËíÖÇ©Ç»Ç¢
            // ãtï˚å¸âÒì]
            float Abs = 360f - (to - from);
            return Abs * -1f;
        }
        else
        {
            // èáï˚å¸
            return to - from;
        }
    }

    // Vectorånìù

    public static Vector2 AddVec(in Vector2 v1, in Vector2 v2)
    {
        return v1 + v2;
    }
    public static Vector2 AddVec(in Vector2 v1, in Vector3 v2)
    {
        return new Vector2(v1.x + v2.x, v1.y + v2.y);
    }
    public static Vector2 AddVec(in Vector3 v1, in Vector2 v2)
    {
        return new Vector2(v1.x + v2.x, v1.y + v2.y);
    }
    public static Vector2 AddVec(in Vector3 v1, in Vector3 v2)
    {
        return new Vector2(v1.x + v2.x, v1.y + v2.y);
    }

    public static Vector2 ToVector(float Degree)
    {
        float Rad = Degree * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(Rad), Mathf.Sin(Rad));
    }
    public static float ToDegree(in Vector2 vector)
    {
        Vector2 dir = vector.normalized;
        float degree = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float normalizedDegree = UnwindDegree(degree);
        return normalizedDegree;
    }
    public static Vector2 RotateVector(in Vector2 vector, float degree)
    {
        Quaternion quat = Quaternion.Euler(0f, 0f, degree);
        return quat * vector;
    }

    public static bool IsSameDirection(in Vector2 a, in Vector2 b)
    {
        float cos = a.Cosine(b);
        return cos > 0f;
    }
    public static bool IsInverseDirection(in Vector2 a, in Vector2 b)
    {
        float cos = a.Cosine(b);
        return cos < 0f;
    }

    public static Vector2 Lerp(in Vector2 a, in Vector2 b, float alpha)
    {
        return Vector2.Lerp(a, b, alpha);
    }
    public static float Lerp(float a, float b, float alpha)
    {
        return Mathf.Lerp(a, b, alpha);
    }
}

[System.Serializable]
public struct SltIntInterval
{
    public SltIntInterval(int min, int max)
    {
        bEnableMin = bEnableMax = true;
        _min = min;
        _max = max;
    }
    public void Reset()
    {
        bEnableMin = bEnableMax = false;
        _min = -int.MaxValue;
        _max = int.MaxValue;
    }
    public void Set(int min, int max)
    {
        this = new SltIntInterval(min, max);
    }

    public void SetMin(int min)
    {
        bEnableMin = true;
        _min = min;
        Validate();
    }
    public void SetMax(int max)
    {
        _max = max;
        Validate();
    }

    public void Clamp(ref int clamped)
    {
        Validate();

        if (bEnableMin && bEnableMax)
        {
            clamped = Mathf.Clamp(clamped, _min, _max);

        }
        else if (bEnableMin && !bEnableMax)
        {
            // ç≈è¨ílÇæÇØë∂ç›
            if (clamped < _min)
            {
                clamped = _min;
            }
        }
        else if (!bEnableMin && bEnableMax)
        {
            // ç≈ëÂílÇæÇØë∂ç›
            if (clamped > _max)
            {
                clamped = _max;
            }
        }
        else
        {
            // âΩÇ‡ÇµÇ»Ç¢
        }
    }

    public int GetRandom()
    {
        return Random.Range(_min, _max);
    }

    void Validate()
    {
        if (bEnableMin && bEnableMax)
        {
            if (_min > _max || _max < _min)
            {
                _min = _max;
            }
        }
    }


    [SerializeField]
    [ToggleLeft]
    [LabelText("Enable Min")]
    bool bEnableMin;
    [SerializeField]
    [ShowIf("@bEnableMin")]
    int _min;

    [SerializeField]
    [ToggleLeft]
    [LabelText("Enable Max")]
    bool bEnableMax;
    [SerializeField]
    [ShowIf("@bEnableMax")]
    int _max;
}

[System.Serializable]
public struct SltFloatInterval
{
    public SltFloatInterval(float min, float max)
    {
        bEnableMin = bEnableMax = true;
        _min = min;
        _max = max;
    }
    public void Reset()
    {
        bEnableMin = bEnableMax = false;
        _min = -float.MaxValue;
        _max = float.MaxValue;
    }
    public void Set(float min, float max)
    {
        this = new SltFloatInterval(min, max);
    }

    public void SetMin(float min)
    {
        bEnableMin = true;
        _min = min;
        Validate();
    }
    public void SetMax(float max)
    {
        _max = max;
        Validate();
    }

    public void Clamp(ref float clamped)
    {
        Validate();

        if (bEnableMin && bEnableMax)
        {
            clamped = Mathf.Clamp(clamped, _min, _max);

        }
        else if (bEnableMin && !bEnableMax)
        {
            // ç≈è¨ílÇæÇØë∂ç›
            if (clamped < _min)
            {
                clamped = _min;
            }
        }
        else if (!bEnableMin && bEnableMax)
        {
            // ç≈ëÂílÇæÇØë∂ç›
            if (clamped > _max)
            {
                clamped = _max;
            }
        }
        else
        {
            // âΩÇ‡ÇµÇ»Ç¢
        }
    }

    public float GetRandom()
    {
        Assert.IsTrue(IsClosedInterval());
        return Random.Range(_min, _max);
    }
    public bool IsClosedInterval()
    {
        return bEnableMin && bEnableMax;
    }
    void Validate()
    {
        if (bEnableMin && bEnableMax)
        {
            if (_min > _max || _max < _min)
            {
                _min = _max;
            }
        }
    }


    [SerializeField]
    [ToggleLeft]
    [LabelText("Enable Min")]
    bool bEnableMin;
    [SerializeField]
    [ShowIf("@bEnableMin")]
    float _min;

    [SerializeField]
    [ToggleLeft]
    [LabelText("Enable Max")]
    bool bEnableMax;
    [SerializeField]
    [ShowIf("@bEnableMax")]
    float _max;
}