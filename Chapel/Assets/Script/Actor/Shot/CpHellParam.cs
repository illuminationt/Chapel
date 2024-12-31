using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Sirenix.Utilities;

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
    [LabelText("���ˉ�")]
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

// �P��̔��˃p�����[�^
[System.Serializable]
public class CpHellParamOneTrigger
{
    [LabelText("���ʕ����̎Z�o���@")]
    public ECpHellForwardType forwardType;
    [LabelText("���ʕ�������̌Œ�p�x����(�x)")]
    public float forwardDegreeOffset;

    [LabelText("N-Way�e��")]
    public int multiwayNum = 1;

    [LabelText("N-Way�e�̔��ˊԊu�p�x")]
    public float multiwayDegInterval = 0f;

    [LabelText("�O��̔��˂���̊p�x����(�x)")]
    public float degreeSubFromPrevShoot = 0f;

    // 
    public float speed = 10f;
    public float speedSub = 0f;
    public SltFloatInterval speedMinMax;

    public float accel = 0f;
    public float accelSub = 0f;

    public float angleSpeed = 0f;
    public float angleAccel = 0f;
}

public enum ECpHellLocationType
{
    Offset,
}
// �e�۔��ˊJ�n���W�֘A�p�����[�^
[System.Serializable]
public class CpHellParamLocation
{
    public Vector2 GetLocation(in Vector2 selfPosition)
    {
        return selfPosition;
    }

    [SerializeField] ECpHellLocationType LocationType;
    [SerializeField]
    [ShowIf("LocationType", ECpHellLocationType.Offset)]
    float OffsetRadius = 0f;
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

public class CpHellTimingWatcher
{
    public CpHellTimingWatcher(int numInUnit, float intervalSec, float intervalSecSub, SltFloatInterval intervalMinMax, float firstDelay)
    {
        // ���炩���߃������m��
        shootTimingList = new List<float>(numInUnit);

        _timer = 0f;
        _latestShootTimingIndex = -1;

        float shootTiming = firstDelay;
        shootTimingList.Add(shootTiming);
        for (int shotIndex = 1; shotIndex < numInUnit; shotIndex++)
        {
            float deltaShootTime = intervalSec + intervalSecSub * (shotIndex - 1);
            shootTiming += deltaShootTime;
            intervalMinMax.Clamp(ref shootTiming);
            shootTimingList.Add(shootTiming);
        }
    }

    public CpHellTimingWatcher(CpHellParamTiming paramTiming) : this(
             paramTiming.num,
             paramTiming.interval,
             paramTiming.intervalSub,
             paramTiming.intervalMinMax,
             paramTiming.FirstDelay)
    {
    }

    public bool Update()
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

    public bool IsFinished()
    {
        return _latestShootTimingIndex >= shootTimingList.Count;
    }

    public int GetCurrentShootCount()
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


    public UnityEvent OnShootTiming = new UnityEvent();
    float _timer = 0f;
    int _latestShootTimingIndex = -1;
    List<float> shootTimingList;
}

public struct FCpUpdateHellContext
{
    public Transform RootTransform;
    public Vector2 InitialPosition;
    public float InitialDegree;
}
public class CpHellUpdator
{
    public CpHellUpdator(CpHellParam hellParam, in FCpUpdateHellContext context)
    {
        _hellParam = hellParam;
        _context = context;

        _timingWatcher = new CpHellTimingWatcher(hellParam.paramTiming);
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
        Vector2 selfPosition = _context.RootTransform.position;
        Vector2 spawnPosition = paramLocation.GetLocation(selfPosition);

        // �v�[������擾
        CpEnemyShot shotPrefab = paramObject.Get();
        CpEnemyShot newShot = CpObjectPool.Get().Create<CpEnemyShot>(shotPrefab);

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
        // �e�۔��˕����̊�ƂȂ�A�O������
        Vector2 shootForward = CalcForwardVector();

        for (int multiwayIndex = 0; multiwayIndex < multiwayNum; multiwayIndex++)
        {
            // N-Way�e�̌v�Z�ɂ�����A���ʊp�x����̊p�x����
            float deltaDegreeOnMultiway = multiwayIndex * paramTrigger.multiwayDegInterval - (paramTrigger.multiwayDegInterval / 2f) * (multiwayNum - 1);

            // �e���̌v�Z�ɂ�����A���ʊp�x����̊p�x����
            int shootCount = _timingWatcher.GetCurrentShootCount();
            float deltaDegreeOnShootCount = shootCount * paramTrigger.degreeSubFromPrevShoot;

            float totalDeltaDegree = deltaDegreeOnMultiway + deltaDegreeOnShootCount;
            Vector2 newDirection = Quaternion.AngleAxis(totalDeltaDegree, Vector3.forward) * shootForward;
            shootDirections.Add(newDirection);
        }

        // ���˃X�s�[�h���Z�o
        int currentShootCount = _timingWatcher.GetCurrentShootCount();
        float speed = paramTrigger.speed + paramTrigger.speedSub * currentShootCount;
        float accel = paramTrigger.accel + paramTrigger.accelSub * currentShootCount;
        for (int multiwayIndex = 0; multiwayIndex < multiwayNum; multiwayIndex++)
        {
            CpEnemyShotInitializeParam initParam = new CpEnemyShotInitializeParam();
            FCpMoveParamEnemyShotDefault moveParamDefault = new FCpMoveParamEnemyShotDefault(
                speed, paramTrigger.speedMinMax, accel,
                paramTrigger.angleSpeed, paramTrigger.angleAccel, shootDirections[multiwayIndex]);

            FCpMoveParamEnemyShot moveParam = default;
            moveParam.MoveType = ECpEnemyShotMoveType.Default;
            moveParam.ParamDefault = moveParamDefault;
            initParam.enemyShotMoveParam = moveParam;
            outParamList.Add(initParam);
        }

        return true;
    }

    Vector2 CalcForwardVector()
    {
        Vector2 retForward = Vector2.zero;
        switch (paramTrigger.forwardType)
        {
            case ECpHellForwardType.Fixed:
                {
                    float degree = _context.InitialDegree + paramTrigger.forwardDegreeOffset;
                    retForward = SltMath.ToVector(degree);
                }
                break;

            case ECpHellForwardType.AimPlayerAll:
                retForward = CalcDirectionToPlayer();
                break;

            case ECpHellForwardType.AimPlayerFirst:
                {
                    int currentShootCount = _timingWatcher.GetCurrentShootCount();
                    if (currentShootCount == 0)
                    {
                        _firstShootDir = CalcDirectionToPlayer();
                    }
                    retForward = _firstShootDir;
                }
                break;

            case ECpHellForwardType.AimPlayerMiddle:
                {
                    int currentShootCount = _timingWatcher.GetCurrentShootCount();
                    if (currentShootCount == 0)
                    {
                        Vector2 toPlayer = CalcDirectionToPlayer();
                        float deltaAngleToFirstDirection = (-paramTrigger.degreeSubFromPrevShoot / 2f) * (paramTiming.num - 1);
                        _firstShootDir = Quaternion.AngleAxis(deltaAngleToFirstDirection, Vector3.forward) * toPlayer;
                    }
                    retForward = _firstShootDir;
                }
                break;
        }

        return retForward;
    }

    Vector2 CalcDirectionToPlayer()
    {
        Vector2 selfPosition = paramLocation.GetLocation(_context.InitialPosition);
        Vector2 toPlayer = CpUtil.GetDirectionToPlayer(selfPosition);
        return toPlayer;
    }


    FCpUpdateHellContext _context;
    CpHellParamObject paramObject => _hellParam.paramObject;
    CpHellParamTiming paramTiming => _hellParam.paramTiming;
    CpHellParamOneTrigger paramTrigger => _hellParam.paramOneTrigger;
    CpHellParamLocation paramLocation => _hellParam.paramLocation;
    CpHellParam _hellParam;

    // �X�V���p�����[�^
    // ���ˎ��R�[���o�b�N
    public UnityEvent OnShoot = new UnityEvent();
    CpHellTimingWatcher _timingWatcher;
    Vector2 _firstShootDir = Vector2.zero;
}
