using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix;
using UnityEngine.Assertions;

//public enum ECpItemDropBehaviorType
//{
//    None = -1,

//    MovePhysical = 1,
//}

//// アイテム生成後の挙動関連のパラメータ
//[System.Serializable]
//public class CpItemDropBehaviorParam
//{
//    public bool IsValidParam()
//    {
//        if (BehaviorType == ECpItemDropBehaviorType.None)
//        {
//            return false;
//        }

//        return true;
//    }
//    public ECpItemDropBehaviorType BehaviorType;

//    [ShowIf("BehaviorType", ECpItemDropBehaviorType.MovePhysical)]
//    public CpItemDropBehaviorParamMovePhysical ParamMovePhysical;
//}

//[System.Serializable]
//public class CpItemDropParamElement
//{
//    public bool IsValidParam()
//    {
//        if (itemPrefab == null)
//        {
//            return false;
//        }
//        if (!behaviorParam.IsValidParam())
//        {
//            return false;
//        }

//        return true;
//    }
//    public GameObject itemPrefab = null;

//    public CpItemDropBehaviorParam behaviorParam = null;
//}

// 落とすアイテムを選ぶパラメータ
public enum ECpItemDropSelectType
{
    None = 0,

    FixedPrefab = 1,
    RandomWeight = 2,
}

[System.Serializable]
public class CpItemDropSelectParamFixedPrefab
{
    public GameObject Prefab = null;
}

[SerializeField]
public class CpItemDropSelectParamRandomWeightElement : ISltLotteryable
{
    public int GetWeight() { return Weight; }
    public GameObject Prefab = null;
    [SerializeField, Min(0)]
    int Weight = 0;
}

[System.Serializable]
public class CpItemDropSelectParamRandomWeight
{
    public GameObject GetPrefab()
    {
        CpItemDropSelectParamRandomWeightElement elem = SltLottery.Get(Elements);
        return elem.Prefab;
    }

    [SerializeField] List<CpItemDropSelectParamRandomWeightElement> Elements = new List<CpItemDropSelectParamRandomWeightElement>();
}

[System.Serializable]
public class CpItemDropSelectParam
{
    public GameObject GetPrefab()
    {
        return SelectType switch
        {
            ECpItemDropSelectType.FixedPrefab => ParamFixedPrefab.Prefab,
            ECpItemDropSelectType.RandomWeight => ParamRandomWeight.GetPrefab(),
            _ => null,
        };
    }
    [SerializeField]
    ECpItemDropSelectType SelectType = ECpItemDropSelectType.None;

    [SerializeField, ShowIf("SelectType", ECpItemDropSelectType.FixedPrefab)]
    CpItemDropSelectParamFixedPrefab ParamFixedPrefab;

    [SerializeField, ShowIf("SelectType", ECpItemDropSelectType.RandomWeight)]
    CpItemDropSelectParamRandomWeight ParamRandomWeight;
}

public enum ECpItemDropType
{
    None = 0,

    RandomScatter = 1,// ランダムにばらまく
}

public struct FCpItemDropRandomScatterRequestParam
{
    public Vector2 InitialPosition;
    public float InitialDegree;
    public FCpMoveParamPhysical MoveParamPhysical;
}

[System.Serializable]
public class CpItemDropParamRandomScatter
{
    public CpItemDropSelectParam SelectParam = null;

    public SltIntInterval NumInterval;

    public FCpItemDropRandomScatterRequestParam CreateRequestParam(in Vector2 basePosition, in Vector2 forward)
    {
        FCpItemDropRandomScatterRequestParam retContext;

        // 初期座標
        Vector2 offsetDir = SltMath.ToVector(ScatterDegreeOffsetInterval.GetRandom());
        float offsetRadius = OffsetRadiusInterval.GetRandom();
        retContext.InitialPosition = basePosition + offsetDir * offsetRadius;

        FCpMoveParamPhysical moveParamPhysical;
        moveParamPhysical.Speed = SpeedInterval.GetRandom();
        moveParamPhysical.Accel = AccelInterval.GetRandom();
        moveParamPhysical.DegreeOffset = ScatterDegreeOffsetInterval.GetRandom();
        moveParamPhysical.RestitutionCoefficient = RestitutionCoefficientInterval.GetRandom();
        retContext.MoveParamPhysical = moveParamPhysical;

        // 初期方向
        retContext.InitialDegree = SltMath.ToDegree(forward) + ScatterDegreeOffsetInterval.GetRandom();

        return retContext;
    }

    // 初期位置の、オーナーからのオフセット半径範囲
    public SltFloatInterval OffsetRadiusInterval;

    // 初速の範囲
    public SltFloatInterval SpeedInterval;
    // 加速度の範囲
    public SltFloatInterval AccelInterval;
    // ばらまく方向の、オーナーからのオフセット範囲
    public SltFloatInterval ScatterDegreeOffsetInterval;
    // アイテムの反発係数範囲
    public SltFloatInterval RestitutionCoefficientInterval;
}

[System.Serializable]
public class CpItemDropParam
{
    public bool IsValidParam()
    {
        return true;
    }

    public ECpItemDropType ItemDropType = ECpItemDropType.None;

    [SerializeField, ShowIf("ItemDropType", ECpItemDropType.RandomScatter)]
    public CpItemDropParamRandomScatter ParamRandomScatter = null;
}

[System.Serializable]
public struct FCpItemDropContext
{
    public Vector2 Position;
    public Vector2 InitialVelocity;
    public float Accel;
}

public class CpItemDropper : MonoBehaviour
{
    public static CpItemDropper Create(Transform ownerTransform, CpItemDropParam param)
    {
        CpItemDropper dropper = MonoBehaviour.Instantiate<CpItemDropper>(CpGameSettings.Get().PrefabSettings.ItemDropperPrefab);
        dropper.transform.position = ownerTransform.position;
        dropper.transform.eulerAngles = ownerTransform.eulerAngles;

        FCpItemDropContext context = new FCpItemDropContext();
        context.Position = ownerTransform.position;
        dropper.RequestDropItem(param, context);

        return dropper;
    }
    public void RequestDropItem(CpItemDropParam param, in FCpItemDropContext context)
    {
        _param = param;
        _context = context;

        RequestDropItemInternal();
    }

    void RequestDropItemInternal()
    {
        switch (_param.ItemDropType)
        {
            case ECpItemDropType.RandomScatter:
                RequestDropItem_RandomScatter(_param.ParamRandomScatter);
                break;
            default:
                Assert.IsTrue(false);
                break;
        }
    }

    void RequestDropItem_RandomScatter(CpItemDropParamRandomScatter paramRandomScatter)
    {
        CpObjectPool objectPool = CpObjectPool.Get();

        int num = paramRandomScatter.NumInterval.GetRandom();
        CpItemDropSelectParam selectParam = paramRandomScatter.SelectParam;

        Vector2 ownerPosition = transform.position;
        Vector2 forward = transform.GetForwardVector();

        for (int index = 0; index < num; index++)
        {
            GameObject prefab = selectParam.GetPrefab();
            GameObject itemObj = objectPool.Create(prefab);

            CpDropItem dropItem = itemObj.GetComponent<CpDropItem>();
            FCpItemDropRandomScatterRequestParam reqParam = paramRandomScatter.CreateRequestParam(ownerPosition, forward);
            dropItem.RequestStartBehavior(reqParam);
        }
    }

    CpItemDropParam _param;
    FCpItemDropContext _context;
}
