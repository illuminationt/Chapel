using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SltUtil
{
    public static void AddToPosition(Transform transform, in Vector2 delta)
    {
        Vector3 pos = transform.position;
        pos.x += delta.x;
        pos.y += delta.y;
        transform.position = pos;
    }
}