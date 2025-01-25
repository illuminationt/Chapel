using UnityEngine;

public static class CpRandom
{
    public static float GetDelta(float variation)
    {
        return Random.Range(-variation / 2f, variation / 2f);
    }

    public static float Range(float min, float max)
    {
        return Random.Range(min, max);
    }

    public static bool Bool()
    {
        int rand = Random.Range(0, 1);
        return rand == 0 ? false : true;
    }

    public static float Sign()
    {
        return Bool() ? 1f : 0f;
    }

    // wrapper
    public static Vector2 insideUnitCircle => Random.insideUnitCircle;
}
