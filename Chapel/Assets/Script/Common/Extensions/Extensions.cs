using UnityEngine;

public static class Vector2Extensions
{
    // 距離関連
    public static float GetDistanceTo(this in Vector2 self, in Vector2 target)
    {
        return Mathf.Sqrt(self.GetSquaredDistanceTo(target));
    }
    public static float GetDistanceTo(this in Vector2 self, in Vector3 target)
    {
        return Mathf.Sqrt(self.GetSquaredDistanceTo(target));
    }
    public static float GetSquaredDistanceTo(this in Vector2 self, in Vector2 target)
    {
        return (self - target).sqrMagnitude;
    }
    public static float GetSquaredDistanceTo(this in Vector2 self, in Vector3 target)
    {
        Vector2 delta = new Vector2(self.x - target.x, self.y - target.y);
        return delta.sqrMagnitude;
    }

    // 方向関連
    public static Vector2 GetDirectionTo(this in Vector2 self, in Vector2 target)
    {
        Vector2 toTarget = (target - self).normalized;
        return toTarget;
    }
    public static Vector2 GetDirectionTo(this in Vector2 self, in Vector3 target)
    {
        Vector2 toTarget = new Vector2(target.x - self.x, target.y - self.y);
        if (toTarget.IsNearlyZero())
        {
            return Vector2.left;
        }
        return toTarget.normalized;
    }

    // ベクトル回転
    public static Vector2 Rotate(this in Vector2 self, float deltaYaw)
    {
        return SltMath.RotateVector(self, deltaYaw);
    }

    public static bool IsNearlyZero(this in Vector2 self, float tolerance = 0.01f)
    {
        float size = self.magnitude;
        return size < tolerance;
    }

    public static SltFloatInterval ToInterval(this in Vector2 self)
    {
        return new SltFloatInterval(self.x, self.y);
    }
    public static float GetRandom(this in Vector2 self)
    {
        return self.ToInterval().GetRandom();
    }

    // 演算
    public static float Dot(this in Vector2 self, in Vector2 other)
    {
        return self.x * other.x + self.y * other.y;
    }
    public static float Cross(this in Vector2 self, in Vector2 other)
    {
        return self.x * other.y - self.y * other.x;
    }
    public static float Cosine(this in Vector2 self, in Vector2 other)
    {
        Vector2 nSelf = self.normalized;
        Vector2 nOther = other.normalized;
        float cos = nSelf.Dot(nOther);
        return cos;
    }
    // selfとotherのなす角度（度数法）を取得
    public static float GetDegreeTo(this in Vector2 self, in Vector2 other)
    {
        float cos = self.Cosine(other);
        float rad = Mathf.Acos(cos);
        return rad * Mathf.Rad2Deg;
    }

    // selfからotherの回転が時計回りか判定
    public static bool IsClockwise(this in Vector2 self, in Vector2 other)
    {
        float cross = self.Cross(other);
        return cross < 0f;
    }

    // 便利
    public static bool IsSameDirection(this in Vector2 self, in Vector2 other)
    {
        return SltMath.IsSameDirection(self, other);
    }
    public static bool IsInverseDirection(this in Vector2 self, in Vector2 other)
    {
        return SltMath.IsInverseDirection(self, other);
    }
}

public static class Vector3Extensions
{
}