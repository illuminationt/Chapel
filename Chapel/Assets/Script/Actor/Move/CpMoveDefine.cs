using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using Unity.VisualScripting;

[Inspectable]
[SerializeField]
public class CpMoveTrajectoryOnHitWallParam
{
    public bool GetNewVelocityOnHitWall(Transform owner, in Vector2 currentVelocity, Collision2D collision, ref Vector2 newVel)
    {
        switch (_trajectory)
        {
            case ECpMoveTrajectoryOnHitWall.Default:
                return false;
            case ECpMoveTrajectoryOnHitWall.Reflect:
                return GetNewDirectionOnHitWall_Reflect(owner, currentVelocity, collision, ref newVel);
            default:
                Assert.IsTrue(false, $"invalid trajectory type detected");
                return false;
        }
    }

    bool GetNewDirectionOnHitWall_Reflect(Transform owner, in Vector2 currentVelocity, Collision2D collision, ref Vector2 newVel)
    {
        // 壁に衝突した後の角度を算出
        // まず法線方向を取得
        ContactPoint2D contactPoint = collision.contacts[0];
        Vector2 impactNormal = contactPoint.normal;
        //Vector2 impactNormal = ((Vector2)owner.position - contactPoint.point).normalized;

        // もともとの移動方向と法線が同じ向きなら反射しない
        if (currentVelocity.IsSameDirection(impactNormal))
        {
            return false;
        }

        Vector2 currentDir = currentVelocity.normalized;
        float currentSpeed = currentVelocity.magnitude;
        // 跳ね返った後の角度を算出
        Vector2 reflectedDir = Vector2.Reflect(currentDir, impactNormal);

        // 壁に衝突した後のスピードを算出
        float newSpeed = currentSpeed * RestitutionCoefficient;

        newVel = reflectedDir * newSpeed;
        return true;
    }

    public enum ECpMoveTrajectoryOnHitWall
    {
        None = 0,// 不正値
        Default, // 壁に衝突したら壁に沿うように移動.特に何も処理しないとこうなるはず
        Reflect, // 反発
    }

    [Inspectable, SerializeField] ECpMoveTrajectoryOnHitWall _trajectory = ECpMoveTrajectoryOnHitWall.None;

    [Inspectable, SerializeField]
    [ShowIf("_trajectory", ECpMoveTrajectoryOnHitWall.Reflect)]
    float RestitutionCoefficient = 1f;
}