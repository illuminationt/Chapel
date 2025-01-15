using UnityEngine;

public static class CpRandom
{
    public static float GetDelta(float variation)
    {
        return Random.Range(-variation / 2f, variation / 2f);
    }
}
