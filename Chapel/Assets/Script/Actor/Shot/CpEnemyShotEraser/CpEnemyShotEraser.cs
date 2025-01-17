using NUnit.Framework;
using UnityEngine;

public class CpEnemyShotEraser
{
    public static CpEnemyShotEraser Create()
    {
        CpEnemyShotEraser eraser = new CpEnemyShotEraser();
        return eraser;
    }
    public static CpEnemyShotEraser Get() { return CpGameManager.Instance.EnemyShotEraser; }

    public void RequestErase()
    {
        Assert.IsTrue(false, $"’eÁ‚µ‚Í–¢À‘•");
    }
    public void RequestErase(float duration)
    {
        if (duration > _eraseTimer)
        {
            _eraseTimer = duration;
        }
    }
    public void Update()
    {
        if (_eraseTimer > 0f)
        {
            _eraseTimer -= Time.deltaTime;
        }
    }

    public bool IsErasing()
    {
        return _eraseTimer > 0f;
    }

    float _eraseTimer = -1f;
}
