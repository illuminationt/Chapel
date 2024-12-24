using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SltUtil
{
    public static class Math
    {
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
    }
}
