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
// 実際に生成するオブジェクト
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

// 弾発射タイミング
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
            Assert.IsTrue(false, $"numは1以上または-1を指定する必要があります");
        }

        return newWatcher;
    }
    [LabelText("発射回数(-1なら無限回)")]
    public int num = 1;
    [LabelText("最初の発射までの時間(秒)")]
    public float FirstDelay = 0f;

    [LabelText("発射時間間隔(秒)")]
    public float interval = 0f;
    [LabelText("前回の発射からの発射時間間隔の変化量(秒)")]
    public float intervalSub = 0f;
    [LabelText("発射時間間隔の最小最大(秒)")]
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
    Noop,// 何もしない
    VelocityDirection,// 速度方向
}

// １回の発射パラメータ
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

    [LabelText("正面方向の算出方法")]
    public ECpHellForwardType forwardType;
    [SerializeField, LabelText("正面方向からの固定角度差分(度)")]
    float forwardDegreeOffset = 0f;
    [SerializeField, LabelText("正面方向からの角度差分ランダム幅(度)")]
    float forwardDegreeOffsetVariation = 0f;

    [LabelText("N-Way弾数")]
    public int multiwayNum = 1;

    [LabelText("N-Way弾の発射間隔角度")]
    public float multiwayDegInterval = 0f;

    [LabelText("前回の発射からの角度差分(度)")]
    public float degreeSubFromPrevShoot = 0f;

    // 
    [LabelText("スピード")]
    public float speed = 10f;
    public float speedSub = 0f;
    public SltFloatInterval speedMinMax;
    [LabelText("スピードに加えるランダム幅")]
    public float speedVariation = 0f;

    public float accel = 0f;
    public float accelSub = 0f;

    public float angleSpeed = 0f;
    public float angleAccel = 0f;

    // スケール
    public SltFloatInterval ScaleInterval;
}

public enum ECpHellLocationType
{
    RootLocation = 0,// Transformの位置をそのまま使う
    Offset = 1,// Transformからのオフセットを姉弟
}

public enum ECpHellLocationRandomOffsetType
{
    None = 0,
    OffsetX = 1,
    OffsetY = 2,
    Rectangle = 3,//正方形の中でランダム範囲
    Circle = 10,//円の中のランダム範囲
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
                Assert.IsTrue(false, $"存在しないOffsetTypeが指定されています:{SltEnumUtil.ToString(RandomOffsetType)}");
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

// 弾丸発射開始座標関連パラメータ
[System.Serializable]
public class CpHellParamLocation
{
    public Vector2 GetLocation(Transform ownerRootTransform)
    {
        // 基本オフセット+ランダムで設定するオフセット
        Vector2 totalOffset = GetBaseOffset() + RandomOffsetParam.GetOffset();

        // 
        float ownerYaw = ownerRootTransform.eulerAngles.z;
        Vector2 rotatedTotalOffset = totalOffset.Rotate(ownerYaw);

        return SltMath.AddVec(ownerRootTransform.position, rotatedTotalOffset);
    }
    // ランダム座標を加える前のオフセットを取得
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

// 弾幕の一単位パラメータ
// 複数の場所から出すときは複数のパラメータを用意する
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

// 弾生成タイミング監視ベースクラス
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

// 弾を生成するタイミングを監視するクラス
public class CpFiniteHellTimingWatcher : CpHellTimingWatcherBase
{
    public CpFiniteHellTimingWatcher(int numInUnit, float intervalSec, float intervalSecSub, SltFloatInterval intervalMinMax, float firstDelay, CpHellRequestOption option) :
        base(numInUnit, intervalSec, intervalSecSub, intervalMinMax, firstDelay, option)
    {
        // あらかじめメモリ確保
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

        // 保険
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

        // プールから取得
        CpEnemyShot shotPrefab = paramObject.Get();
        CpEnemyShot newShot = CpObjectPool.Get().Get(shotPrefab);

        // 初期座標を設定
        newShot.transform.position = spawnPosition;

        // 初期化.移動パラメータ等は弾の方で設定する
        newShot.Initialize(initParam);

        return true;
    }

    public bool CreateEnemyShotParamList(out List<CpEnemyShotInitializeParam> outParamList)
    {
        int multiwayNum = paramTrigger.multiwayNum;
        outParamList = new List<CpEnemyShotInitializeParam>(multiwayNum);

        // 発射方向を算出
        List<Vector2> shootDirections = new List<Vector2>(multiwayNum);

        for (int multiwayIndex = 0; multiwayIndex < multiwayNum; multiwayIndex++)
        {
            // N-Way弾の計算における、正面角度からの角度差分
            float deltaDegreeOnMultiway = multiwayIndex * paramTrigger.multiwayDegInterval - (paramTrigger.multiwayDegInterval / 2f) * (multiwayNum - 1);

            // 弾数の計算における、正面角度からの角度差分
            int shootCount = _timingWatcher.GetCurrentShootCount();
            float deltaDegreeOnShootCount = shootCount * paramTrigger.degreeSubFromPrevShoot;

            float optionalDeltaDegree = _option?.DeltaDegree ?? 0f;

            float totalDeltaDegree = deltaDegreeOnMultiway + deltaDegreeOnShootCount + optionalDeltaDegree;



            // 弾の初期位置を算出（角度計算で使うのでここで算出）
            paramLocation.GetLocation(_context.RootTransform);

            // 基準角度に修正を加える（ランダムが存在するので弾ごとに計算する）
            // 弾丸発射方向の基準となる、前方方向
            Vector2 spawnPosition = paramLocation.GetLocation(_context.RootTransform);
            Vector2 shootBaseForward = CalcForwardVector(spawnPosition);
            Vector2 calcedShootForward = SltMath.RotateVector(shootBaseForward, paramTrigger.GetForwardDegreeOffset());

            Vector2 newDirection = Quaternion.AngleAxis(totalDeltaDegree, Vector3.forward) * calcedShootForward;

            CpEnemyShotInitializeParam initParam = new CpEnemyShotInitializeParam();

            // GetSpeed()にランダム要素を含むので弾ごとに計算する
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
                    Assert.IsTrue(paramTiming.num > 0, $"AimPlayerMiddleのnumは姓の値である必要があります");

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

    // 更新中パラメータ
    // 発射時コールバック
    public UnityEvent OnShoot = new UnityEvent();
    CpHellTimingWatcherBase _timingWatcher;
    Vector2 _firstShootDir = Vector2.zero;
}
