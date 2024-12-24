using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oddworm.Framework;

public static class CpDebugUtil
{
    public static void DrawLine(Vector3 start,Vector3 end,Color color,float duration=0f,bool depthTest=true)
    {
        DbgDraw.Line(start, end, color, duration, depthTest);
    }

    public static void DrawArrow(Vector2 start,Vector2 end,float duration,Color color)
    {
        // íºê¸ïîï™
        CpDebugUtil.DrawLine(start, end, color, duration);

        // êÊí[
        Vector2 direction = (end - start).normalized;
        Vector2 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 135, 0) * Vector2.right;
        Vector2 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -135, 0) * Vector2.right;

        float arrowSize = (end - start).magnitude / 5f;
        if (arrowSize < 3f)
        {
            arrowSize = 3;
        }
        CpDebugUtil.DrawLine(end, end + right * arrowSize, color, duration);
        CpDebugUtil.DrawLine(end, end + left * arrowSize, color, duration);
    }
}

