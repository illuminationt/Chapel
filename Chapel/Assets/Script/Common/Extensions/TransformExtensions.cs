using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class TransformExtensions
{
    // getter
    public static Vector2 GetForwardVector(this Transform self)
    {
        float yaw = self.GetYaw();
        return SltMath.ToVector(yaw);
    }
    public static float GetYaw(this Transform self)
    {
        return self.eulerAngles.z;
    }


    public static void AddToPosition(this Transform self, in Vector2 delta)
    {
        self.position += new Vector3(delta.x, delta.y, 0f);
    }

    public static void SetPosition(this Transform self, in Vector2 position)
    {
        self.position = position;
    }

    public static void SetRotation(this Transform self, float yaw)
    {
        Vector3 eulerAngles = self.rotation.eulerAngles;
        eulerAngles.z = yaw;
        self.eulerAngles = eulerAngles;
    }
    public static void AddToRotationZ(this Transform self, float deltaYaw)
    {
        float newDelta = self.eulerAngles.z + deltaYaw;
        self.SetRotation(newDelta);
    }

    public static void SetLocalScale(this Transform self, float scale)
    {
        Vector3 localScale = self.localScale;
        localScale.x = scale;
        localScale.y = scale;
        self.localScale = localScale;
    }

}

