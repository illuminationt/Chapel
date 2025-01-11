using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void AddToPosition(this Transform self, in Vector2 delta)
    {
        self.position += new Vector3(delta.x, delta.y, 0f);
    }
}

