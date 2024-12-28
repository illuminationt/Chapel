using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
