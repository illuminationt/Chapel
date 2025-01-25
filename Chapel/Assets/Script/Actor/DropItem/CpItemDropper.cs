using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix;
using UnityEngine.Assertions;

// ���Ƃ��A�C�e����I�ԃp�����[�^
public enum ECpItemDropSelectType
{
    None = 0,

    FixedPrefab = 1,
    RandomWeight = 2,
}

public enum ECpDropItemRefType
{
    None = 0,
    PrefabGameObject,//GameObject�N���X���g���ĎQ�Ƃ���
    PrefabCpDropItem,//CpDropItem�N���X���g���ĎQ�Ƃ���
}

[System.Serializable]
public class CpItemDropSelectParamFixedPrefab
{
    public CpDropItem Prefab = null;
}

[SerializeField]
public class CpItemDropSelectParamRandomWeightElement : ISltLotteryable
{
    public int GetWeight() { return Weight; }
    public CpDropItem Prefab = null;
    [SerializeField, Min(0)]
    int Weight = 0;
}

[System.Serializable]
public class CpItemDropSelectParamRandomWeight
{
    public CpDropItem GetPrefab()
    {
        CpItemDropSelectParamRandomWeightElement elem = SltLottery.Get(Elements);
        return elem.Prefab;
    }

    [SerializeField] List<CpItemDropSelectParamRandomWeightElement> Elements = new List<CpItemDropSelectParamRandomWeightElement>();
}

[System.Serializable]
public class CpItemDropSelectParam
{
    public CpDropItem GetPrefab()
    {
        return SelectType switch
        {
            ECpItemDropSelectType.FixedPrefab => ParamFixedPrefab.Prefab,
            ECpItemDropSelectType.RandomWeight => ParamRandomWeight.GetPrefab(),

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

    RandomScatter = 1,// �����_���ɂ΂�܂�
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

        // �������W
        Vector2 offsetDir = SltMath.ToVector(ScatterDegreeOffsetInterval.GetRandom());
        float offsetRadius = OffsetRadiusInterval.GetRandom();
        retContext.InitialPosition = basePosition + offsetDir * offsetRadius;

        FCpMoveParamPhysical moveParamPhysical;
        moveParamPhysical.Speed = SpeedInterval.GetRandom();
        moveParamPhysical.Accel = AccelInterval.GetRandom();
        moveParamPhysical.DegreeOffset = ScatterDegreeOffsetInterval.GetRandom();
        moveParamPhysical.RestitutionCoefficient = RestitutionCoefficientInterval.GetRandom();
        retContext.MoveParamPhysical = moveParamPhysical;

        // ��������
        retContext.InitialDegree = SltMath.ToDegree(forward) + ScatterDegreeOffsetInterval.GetRandom();

        return retContext;
    }

    // �����ʒu�́A�I�[�i�[����̃I�t�Z�b�g���a�͈�
    public SltFloatInterval OffsetRadiusInterval;

    // �����͈̔�
    public SltFloatInterval SpeedInterval;
    // �����x�͈̔�
    public SltFloatInterval AccelInterval;
    // �΂�܂������́A�I�[�i�[����̃I�t�Z�b�g�͈�
    public SltFloatInterval ScatterDegreeOffsetInterval;
    // �A�C�e���̔����W���͈�
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
            case ECpItemDropType.None:
                // �A�C�e���h���b�v���Ȃ�
                break;
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

        CpObjectPool pool = CpObjectPool.Get();

        for (int index = 0; index < num; index++)
        {
            CpDropItem prefab = selectParam.GetPrefab();

            CpDropItem dropItem = pool.Get(prefab);
            FCpItemDropRandomScatterRequestParam reqParam = paramRandomScatter.CreateRequestParam(ownerPosition, forward);
            dropItem.RequestStartBehavior(reqParam);

            CpRoomProxyManager roomProxyManager = CpRoomProxyManager.Get();
            CpRoomProxy roomProxy = roomProxyManager.GetActiveRoomProxy();
            CpRoom roomInstance = roomProxy.GetRoomInstance();
        }
    }

    CpItemDropParam _param;
    FCpItemDropContext _context;
}
