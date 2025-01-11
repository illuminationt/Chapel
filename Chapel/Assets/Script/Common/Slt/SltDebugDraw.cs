using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
public static class SltDebugDraw
{
    public static void DrawLine(Vector2 start, Vector2 end, Color color, float thickness = 5f, float duration = 0f, bool depthTest = true)
    {
        using (Draw.Command(Camera.main))
        {
            Draw.LineGeometry = LineGeometry.Flat2D;
            Draw.Thickness = thickness;
            Draw.Color = color;
            Draw.Line(start, end);
        }
    }

    public static void DrawArrow(Vector2 start, Vector2 end, Color color, float thickness = 0.1f, float duration = 0f)
    {
        DrawLine(start, end, color, thickness, duration);

        Vector2 direction = (end - start).normalized;
        float headAngle = 45f;
        Vector2 right = Quaternion.Euler(0, 0, 180 + headAngle) * direction;
        Vector2 left = Quaternion.Euler(0, 0, 180 - headAngle) * direction;

        float headLength = (end - start).magnitude / 10f;

        Vector2 toLeft = end + left * headLength;
        Vector2 toRight = end + right * headLength;
        DrawLine(end, toLeft, color, thickness, duration);
        DrawLine(end, toRight, color, thickness, duration);
    }

    public static void DrawArrow(Vector2 start, Vector2 direction, float length, Color color, float thickness = 2f, float duration = 0f)
    {
        Vector2 end = start + direction * length;
        DrawArrow(start, end, color, thickness, duration);
    }

    public static void DrawText(Vector2 location, string str, Color color, float fontSize = 5f)
    {
        using (Draw.Command(Camera.main))
        {
            Draw.Text(str, fontSize, color);
        }
    }

}

public static class SltDebugDrawOnGizmos
{
    public static void DrawArrow(Vector2 start, Vector2 end, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(start, end);

        Vector3 arrowDir = (end - start).normalized;
        // –îˆó‚Ì“ª‚Ì•ûŒü‚ðŒvŽZ
        float headAngle = 45f;
        Vector2 right = Quaternion.Euler(0, 0, 180 + headAngle) * arrowDir;
        Vector2 left = Quaternion.Euler(0, 0, 180 - headAngle) * arrowDir;

        // –îˆó‚Ì“ª‚ð•`‰æ
        float arrowLength = (end - start).magnitude;
        Gizmos.DrawLine(end, end + right * (arrowLength * 0.2f));
        Gizmos.DrawLine(end, end + left * (arrowLength * 0.2f));
    }

    public static void DrawArrow(in Vector2 start, in Vector2 dir, float length, Color color)
    {
        Vector2 end = start + dir * length;
        DrawArrow(start, end, color);
    }
}