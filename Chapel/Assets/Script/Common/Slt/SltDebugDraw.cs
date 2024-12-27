using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using SltUtil;

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

    public static void DrawText(string str, Color color, float fontSize = 5f)
    {
        using (Draw.Command(Camera.main))
        {
            Draw.Text(str, fontSize, color);
        }
    }
}