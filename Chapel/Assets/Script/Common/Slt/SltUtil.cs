using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SltUtil
{
    public static class SltMath
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

        public static Vector2 AddVec(in Vector2 v1, in Vector2 v2)
        {
            return v1 + v2;
        }
        public static Vector2 AddVec(in Vector2 v1,in Vector3 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vector2 AddVec(in Vector3 v1,in Vector2 v2)
        {
            return new Vector2(v1.x+v2.x, v1.y+v2.y);
        }
        public static Vector2 AddVec(in Vector3 v1, in Vector3 v2)
        {
            return new Vector2(v1.x+v2.x,v1.y+v2.y);
        }
    }
}
