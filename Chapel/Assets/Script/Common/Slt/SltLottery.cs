using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public interface ISltLotteryable
{
    public int GetWeight();
}

public static class SltLottery
{
    public static T Get<T>(IReadOnlyList<T> collection) where T : class, ISltLotteryable
    {
        if (collection == null || collection.Count == 0)
        {
            return null;
        }

        if (collection.Count == 1)
        {
            return collection[0];
        }

        int total = collection.Sum(c => c.GetWeight());
        float random = SltRandom.Range(0f, total);

        float current = 0f;
        for (int index = 0; index < collection.Count; index++)
        {
            current += collection[index].GetWeight();
            if (current >= random)
            {
                return collection[index];
            }
        }

        Assert.IsTrue(false);
        return collection[0];
    }
}