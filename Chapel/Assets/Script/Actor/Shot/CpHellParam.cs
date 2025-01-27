using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Sirenix.Utilities;
using System;
using JetBrains.Annotations;

public enum ECpHellObjectType
{
    Prefab,
    Lottery,
}

[System.Serializable]
public class CpHellObjectLotteryUnit : ISltLotteryable
{
    public int GetWeight() { return LotteryWeight; }
    public CpEnemyShot ShotPrefab;
    public int LotteryWeight;
}

[System.Serializable]
public class CpHellObjectLottery
{
    public CpEnemyShot Get() { return SltLottery.Get(LotteryUnitList).ShotPrefab; }
    public List<CpHellObjectLotteryUnit> LotteryUnitList;
}
// ���ۂɐ�������I�u�W�F�N�g
[System.Serializable]
public class CpHellParamObject
{
    public CpEnemyShot Get()
    {
        return ObjectType switch
        {

            ECpHellObjectType.Prefab => ShotPrefab,
            ECpHellObjectType.Lottery => Lottery.Get(),
            _ => throw new System.Exception(),
        };
    }
    public ECpHellObjectType ObjectType;
    [ShowIf("ObjectType", ECpHellObjectType.Prefab)]
    public CpEnemyShot ShotPrefab;
    [ShowIf("ObjectType", ECpHellObjectType.Lottery)]
    CpHellObjectLottery Lottery;
}

// �e���˃^�C�~���O
[System.Serializable]
public class CpHellParamTiming
{
    public CpHellTimingWatcherBase CreateHellTimingWatcher(CpHellRequestOption option)
    {
        CpHellTimingWatcherBase newWatcher = null;
        if (num > 0)
        {
            newWatcher = new CpFiniteHellTimingWatcher(this, option);
        }
        else if (num == -1)
        {
            newWatcher = new CpInfiniteHellTimingWatcher(this, option);
        }
        else
        {
            Assert.IsTrue(false, $"num��1�ȏ�܂���-1���w�肷��K�v������܂�");
        }

        return newWatcher;
    }
    [LabelText("���ˉ�(-1�Ȃ疳����)")]
    public int num = 1;
    [LabelText("�ŏ��̔��˂܂ł̎���(�b)")]
    public float FirstDelay = 0f;

    [LabelText("���ˎ��ԊԊu(�b)")]
    public float interval = 0f;
    [LabelText("�O��̔��˂���̔��ˎ��ԊԊu�̕ω���(�b)")]
    public float intervalSub = 0f;
    [LabelText("���ˎ��ԊԊu�̍ŏ��ő�(�b)")]
    public SltFloatInterval intervalMinMax;
}

public enum ECpHellForwardType
{
    Fixed,
    AimPlayerAll,
    AimPlayerFirst,
    AimPlayerMiddle,
}

public enum ECpEnemyShotMoveType
{
    Default,
    MoveParam,
    MoveParamList,
}

public enum ECpEnemyShotRotationType
{
    Noop,// �������Ȃ�
    VelocityDirection,// ���x����
}

// �P��̔��˃p�����[�^
[System.Serializable]
public class CpHellParamOneTrigger
{
    public float GetForwardDegreeOffset()
    {
        return forwardDegreeOffset + CpRandom.GetDelta(forwardDegreeOffsetVariation);
    }
    public float GetSpeed()
    {
        return speed + CpRandom.Range(-speedVariation / 2f, speedVariation / 2f);
    }
    public float GetScale()
    {
        if (ScaleInterval.IsClosedInterval())
        {
            return ScaleInterval.GetRandom();
        }
        return 1f;
    }

    [LabelText("���ʕ����̎Z�o���@")]
    public ECpHellForwardType forwardType;
    [SerializeField, LabelText("���ʕ�������̌Œ�p�x����(�x)")]
    float forwardDegreeOffset = 0f;
    [SerializeField, LabelText("���ʕ�������̊p�x���������_����(�x)")]
    float forwardDegreeOffsetVariation = 0f;

    [LabelText("N-Way�e��")]
    public int multiwayNum = 1;

    [LabelText("N-Way�e�̔��ˊԊu�p�x")]
    public float multiwayDegInterval = 0f;

    [LabelText("�O��̔��˂���̊p�x����(�x)")]
    public float degreeSubFromPrevShoot = 0f;

    // 
    [LabelText("�X�s�[�h")]
    public float speed = 10f;
    public float speedSub = 0f;
    public SltFloatInterval speedMinMax;
    [LabelText("�X�s�[�h�ɉ����郉���_����")]
    public float speedVariation = 0f;

    public float accel = 0f;
    public float accelSub = 0f;

    public float angleSpeed = 0f;
    public float angleAccel = 0f;

    // �X�P�[��
    public SltFloatInterval ScaleInterval;
}

public enum ECpHellLocationType
{
    RootLocation = 0,// Transform�̈ʒu�����̂܂܎g��
    Offset = 1,// Transform����̃I�t�Z�b�g���o��
}

public enum ECpHellLocationRandomOffsetType
{
    None = 0,
    OffsetX = 1,
    OffsetY = 2,
    Rectangle = 3,//�����`�̒��Ń����_���͈�
    Circle = 10,//�~�̒��̃����_���͈�
}

[Serializable]
public class CpHellLocationRandomOffsetParamRectangle
{
    public Vector2 GetOffset()
    {
        float x = CpRandom.Range(-Size.x / 2f, Size.x / 2f);
        float y = CpRandom.Range(-Size.y / 2f, Size.y / 2f);
        Vector2 ret = new Vector2(Center.x + x, Center.y + y);
        return ret;
    }
    [SerializeField] Vector2 Center = Vector2.zero;
    [SerializeField] Vector2 Size = Vector2.zero;
}

[Serializable]
public class CpHellLocationRandomOffsetParam
{
    public Vector2 GetOffset()
    {
        switch (RandomOffsetType)
        {
            case ECpHellLocationRandomOffsetType.None:
                return Vector2.zero;
            case ECpHellLocationRandomOffsetType.OffsetX:
                return Vector2.right * OffsetXInterval.GetRandom();
            case ECpHellLocationRandomOffsetType.OffsetY:
                return Vector2.up * OffsetYInterval.GetRandom();
            case ECpHellLocationRandomOffsetType.Rectangle:
                return Rectangle.GetOffset();
            case ECpHellLocationRandomOffsetType.Circle:
                return Radius * CpRandom.insideUnitCircle;
            default:
                Assert.IsTrue(false, $"���݂��Ȃ�OffsetType���w�肳��Ă��܂�:{SltEnumUtil.ToString(RandomOffsetType)}");
                return Vector2.zero;
        }
    }
    [SerializeField]
    ECpHellLocationRandomOffsetType RandomOffsetType = ECpHellLocationRandomOffsetType.None;

    [SerializeField, ShowIf("RandomOffsetType", ECpHellLocationRandomOffsetType.OffsetX)]
    Vector2 OffsetXInterval = Vector2.zero;

    [SerializeField, ShowIf("RandomOffsetType", ECpHellLocationRandomOffsetType.OffsetY)]
    Vector2 OffsetYInterval = Vector2.zero;

    [SerializeField, ShowIf("RandomOffsetType", ECpHellLocationRandomOffsetType.Rectangle)]
    CpHellLocationRandomOffsetParamRectangle Rectangle;

    [SerializeField, ShowIf("RandomOffsetType", ECpHellLocationRandomOffsetType.Circle)]
    float Radius = 0f;

}

// �e�۔��ˊJ�n���W�֘A�p�����[�^
[System.Serializable]
public class CpHellParamLocation
{
    public Vector2 GetLocation(Transform ownerRootTransform)
    {
        // ��{�I�t�Z�b�g+�����_���Őݒ肷��I�t�Z�b�g
        Vector2 totalOffset = GetBaseOffset() + RandomOffsetParam.GetOffset();

        // 
        float ownerYaw = ownerRootTransform.eulerAngles.z;
        Vector2 rotatedTotalOffset = totalOffset.Rotate(ownerYaw);

        return SltMath.AddVec(ownerRootTransform.position, rotatedTotalOffset);
    }
    // �����_�����W��������O�̃I�t�Z�b�g���擾
    Vector2 GetBaseOffset()
    {
        switch (LocationType)
        {
            case ECpHellLocationType.RootLocation:
                return Vector2.zero;

            case ECpHellLocationType.Offset:
                return Vector2.zero + Offset;

            default:
                Assert.IsTrue(false);
                return Vector2.zero;
        }
    }

    [SerializeField]
    ECpHellLocationType LocationType;

    [SerializeField]
    [ShowIf("LocationType", ECpHellLocationType.Offset)]
    Vector2 Offset = Vector2.zero;

    [SerializeField]
    CpHellLocationRandomOffsetParam RandomOffsetParam;
}

// �e���̈�P�ʃp�����[�^
// �����̏ꏊ����o���Ƃ��͕����̃p�����[�^��p�ӂ���
[System.Serializable]
public class CpHellParam
{
    public CpHellParamObject paramObject;
    public CpHellParamLocation paramLocation;
    public CpHellParamTiming paramTiming;
    public CpHellParamOneTrigger paramOneTrigger;
}

public enum ECpHellTimingWatchResult
{
    None,
    Shoot,
    Finished,
}

// �e�����^�C�~���O�Ď��x�[�X�N���X
public abstract class CpHellTimingWatcherBase
{
    public CpHellTimingWatcherBase(int numInUnit, float intervalSec, float intervalSecSub, SltFloatInterval intervalMinMax, float firstDelay, CpHellRequestOption option)
    {

    }

    public CpHellTimingWatcherBase(CpHellParamTiming paramTiming, CpHellRequestOption option) : this(
             paramTiming.num,
             paramTiming.interval,
             paramTiming.intervalSub,
             paramTiming.intervalMinMax,
             paramTiming.FirstDelay,
             option)
    {
    }

    public virtual bool Update()
    {
        return false;
    }

    public virtual bool IsFinished()
    {
        return false;
    }

    public virtual int GetCurrentShootCount() { return -1; }

    public UnityEvent OnShootTiming = new UnityEvent();
}

// �e�𐶐�����^�C�~���O���Ď�����N���X
public class CpFiniteHellTimingWatcher : CpHellTimingWatcherBase
{
    public CpFiniteHellTimingWatcher(int numInUnit, float intervalSec, float intervalSecSub, SltFloatInterval intervalMinMax, float firstDelay, CpHellRequestOption option) :
        base(numInUnit, intervalSec, intervalSecSub, intervalMinMax, firstDelay, option)
    {
        // ���炩���߃������m��
        shootTimingList = new List<float>(numInUnit);

        _timer = 0f;
        _latestShootTimingIndex = -1;

        float shootTiming = firstDelay + (option?.AdditionalFirstDelay ?? 0f);
        shootTimingList.Add(shootTiming);
        for (int shotIndex = 1; shotIndex < numInUnit; shotIndex++)
        {
            float deltaShootTime = intervalSec + intervalSecSub * (shotIndex - 1);
            shootTiming += deltaShootTime;
            intervalMinMax.Clamp(ref shootTiming);
            shootTimingList.Add(shootTiming);
        }
    }

    public CpFiniteHellTimingWatcher(CpHellParamTiming paramTiming, CpHellRequestOption option) : this(
             paramTiming.num,
             paramTiming.interval,
             paramTiming.intervalSub,
             paramTiming.intervalMinMax,
             paramTiming.FirstDelay,
             option)
    {
    }

    public override bool Update()
    {
        if (IsFinished())
        {
            return true;
        }

        _timer += CpTime.DeltaTime;

        int maxShootableIndex = GetMaxShootableTimingIndex();
        Assert.IsTrue(_latestShootTimingIndex <= maxShootableIndex);
        while (_latestShootTimingIndex < maxShootableIndex)
        {
            _latestShootTimingIndex++;
            OnShootTiming.Invoke();
        }

        return false;
    }

    public override bool IsFinished()
    {
        return _latestShootTimingIndex >= shootTimingList.Count;
    }

    public override int GetCurrentShootCount()
    {
        return _latestShootTimingIndex;
    }

    // 
    int GetMaxShootableTimingIndex()
    {
        int maxShootableTimingIndex = -1;
        for (int index = 0; index < shootTimingList.Count; index++)
        {
            if (_timer > shootTimingList[index])
            {
                maxShootableTimingIndex = index + 1;
            }
            else
            {
                break;
            }
        }
        return maxShootableTimingIndex;
    }

    float _timer = 0f;
    int _latestShootTimingIndex = -1;
    List<float> shootTimingList;
}

public class CpInfiniteHellTimingWatcher : CpHellTimingWatcherBase
{
    public CpInfiniteHellTimingWatcher(CpHellParamTiming paramTiming, CpHellRequestOption option) : base(paramTiming, option)
    {
        _param = paramTiming;
        _timer = 0f;
        _nextShootTime = paramTiming.FirstDelay;
    }

    public override bool Update()
    {
        _timer += CpTime.DeltaTime;
        if (_timer > _nextShootTime)
        {
            _timer = 0f;
            _nextShootTime = _param.interval + _param.intervalSub * _shootCounter;
            _param.intervalMinMax.Clamp(ref _nextShootTime);

            OnShootTiming.Invoke();

            _shootCounter++;
        }

        // �ی�
        if (_shootCounter > int.MaxValue - 10)
        {
            _shootCounter = int.MaxValue / 2;
        }

        return false;
    }

    public override bool IsFinished()
    {
        return false;
    }

    public override int GetCurrentShootCount()
    {
        return _shootCounter;
    }

    CpHellParamTiming _param = null;
    float _timer = 0f;
    float _nextShootTime = 0f;
    int _shootCounter = 0;
}

public struct FCpUpdateHellContext
{
    public Transform RootTransform;
    public Vector2 InitialPosition;
    public ICpActorForwardInterface ForwardInterface;
}
public class CpHellUpdator
{
    public CpHellUpdator(CpHellParam hellParam, CpHellRequestOption option, in FCpUpdateHellContext context)
    {
        _hellParam = hellParam;
        _option = option;
        _context = context;

        _timingWatcher = hellParam.paramTiming.CreateHellTimingWatcher(_option);
        _timingWatcher.OnShootTiming.AddListener(OnNotifyShootTiming);
    }

    public bool Update()
    {
        _timingWatcher.Update();
        return true;
    }

    public bool IsFinished()
    {
        return _timingWatcher.IsFinished();
    }

    void OnNotifyShootTiming()
    {
        List<CpEnemyShotInitializeParam> shotParams;
        CreateEnemyShotParamList(out shotParams);

        foreach (CpEnemyShotInitializeParam initParam in shotParams)
        {
            bool bSucceed = CreateEnemyShot(initParam);
            if (!bSucceed)
            {
                return;
            }
        }
    }

    bool CreateEnemyShot(CpEnemyShotInitializeParam initParam)
    {
        Vector2 spawnPosition = paramLocation.GetLocation(_context.RootTransform);

        // �v�[������擾
        CpEnemyShot shotPrefab = paramObject.Get();
        CpEnemyShot newShot = CpObjectPool.Get().Get(shotPrefab);

        // �������W��ݒ�
        newShot.transform.position = spawnPosition;

        // ������.�ړ��p�����[�^���͒e�̕��Őݒ肷��
        newShot.Initialize(initParam);

        return true;
    }

    public bool CreateEnemyShotParamList(out List<CpEnemyShotInitializeParam> outParamList)
    {
        int multiwayNum = paramTrigger.multiwayNum;
        outParamList = new List<CpEnemyShotInitializeParam>(multiwayNum);

        // ���˕������Z�o
        List<Vector2> shootDirections = new List<Vector2>(multiwayNum);

        for (int multiwayIndex = 0; multiwayIndex < multiwayNum; multiwayIndex++)
        {
            // N-Way�e�̌v�Z�ɂ�����A���ʊp�x����̊p�x����
            float deltaDegreeOnMultiway = multiwayIndex * paramTrigger.multiwayDegInterval - (paramTrigger.multiwayDegInterval / 2f) * (multiwayNum - 1);

            // �e���̌v�Z�ɂ�����A���ʊp�x����̊p�x����
            int shootCount = _timingWatcher.GetCurrentShootCount();
            float deltaDegreeOnShootCount = shootCount * paramTrigger.degreeSubFromPrevShoot;

            float optionalDeltaDegree = _option?.DeltaDegree ?? 0f;

            float totalDeltaDegree = deltaDegreeOnMultiway + deltaDegreeOnShootCount + optionalDeltaDegree;



            // �e�̏����ʒu���Z�o�i�p�x�v�Z�Ŏg���̂ł����ŎZ�o�j
            paramLocation.GetLocation(_context.RootTransform);

            // ��p�x�ɏC����������i�����_�������݂���̂Œe���ƂɌv�Z����j
            // �e�۔��˕����̊�ƂȂ�A�O������
            Vector2 spawnPosition = paramLocation.GetLocation(_context.RootTransform);
            Vector2 shootBaseForward = CalcForwardVector(spawnPosition);
            Vector2 calcedShootForward = SltMath.RotateVector(shootBaseForward, paramTrigger.GetForwardDegreeOffset());

            Vector2 newDirection = Quaternion.AngleAxis(totalDeltaDegree, Vector3.forward) * calcedShootForward;

            CpEnemyShotInitializeParam initParam = new CpEnemyShotInitializeParam();

            // GetSpeed()�Ƀ����_���v�f���܂ނ̂Œe���ƂɌv�Z����
            int currentShootCount = _timingWatcher.GetCurrentShootCount();
            float accel = paramTrigger.accel + paramTrigger.accelSub * currentShootCount;
            float speed = paramTrigger.GetSpeed() + paramTrigger.speedSub * currentShootCount;

            FCpMoveParamEnemyShotDefault moveParamDefault = new FCpMoveParamEnemyShotDefault(
                speed, paramTrigger.speedMinMax, accel,
                paramTrigger.angleSpeed, paramTrigger.angleAccel, newDirection);

            FCpMoveParamEnemyShot moveParam = default;
            moveParam.MoveType = ECpEnemyShotMoveType.Default;
            moveParam.ParamDefault = moveParamDefault;
            initParam.enemyShotMoveParam = moveParam;
            initParam.Scale = paramTrigger.GetScale();

            outParamList.Add(initParam);

            //shootDirections.Add(newDirection);
        }

        return true;
    }

    Vector2 CalcForwardVector(in Vector2 selfPosition)
    {
        Vector2 retForward = Vector2.zero;
        switch (paramTrigger.forwardType)
        {
            case ECpHellForwardType.Fixed:
                {
                    float forwardDegree = _context.ForwardInterface.GetForwardDegree();
                    retForward = SltMath.ToVector(forwardDegree);
                }
                break;

            case ECpHellForwardType.AimPlayerAll:
                retForward = CalcDirectionToPlayer(selfPosition);
                break;

            case ECpHellForwardType.AimPlayerFirst:
                {
                    int currentShootCount = _timingWatcher.GetCurrentShootCount();
                    if (currentShootCount == 0)
                    {
                        _firstShootDir = CalcDirectionToPlayer(selfPosition);
                    }
                    retForward = _firstShootDir;
                }
                break;

            case ECpHellForwardType.AimPlayerMiddle:
                {
                    Assert.IsTrue(paramTiming.num > 0, $"AimPlayerMiddle��num�͐��̒l�ł���K�v������܂�");

                    int currentShootCount = _timingWatcher.GetCurrentShootCount();
                    if (currentShootCount == 0)
                    {
                        Vector2 toPlayer = CalcDirectionToPlayer(selfPosition);
                        float deltaAngleToFirstDirection = (-paramTrigger.degreeSubFromPrevShoot / 2f) * (paramTiming.num - 1);
                        _firstShootDir = Quaternion.AngleAxis(deltaAngleToFirstDirection, Vector3.forward) * toPlayer;
                    }
                    retForward = _firstShootDir;
                }
                break;
        }

        return retForward;
    }

    Vector2 CalcDirectionToPlayer(in Vector2 selfPosition)
    {
        Vector2 toPlayer = CpUtil.GetDirectionToPlayer(selfPosition);
        return toPlayer;
    }

    FCpUpdateHellContext _context;
    CpHellParamObject paramObject => _hellParam.paramObject;
    CpHellParamTiming paramTiming => _hellParam.paramTiming;
    CpHellParamOneTrigger paramTrigger => _hellParam.paramOneTrigger;
    CpHellParamLocation paramLocation => _hellParam.paramLocation;
    CpHellParam _hellParam;
    CpHellRequestOption _option = null;

    // �X�V���p�����[�^
    // ���ˎ��R�[���o�b�N
    public UnityEvent OnShoot = new UnityEvent();
    CpHellTimingWatcherBase _timingWatcher;
    Vector2 _firstShootDir = Vector2.zero;
}
