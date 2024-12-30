using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;

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