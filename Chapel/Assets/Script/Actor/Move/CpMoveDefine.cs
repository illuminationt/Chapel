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
        // �ǂɏՓ˂�����̊p�x���Z�o
        // �܂��@���������擾
        ContactPoint2D contactPoint = collision.contacts[0];
        Vector2 impactNormal = contactPoint.normal;
        //Vector2 impactNormal = ((Vector2)owner.position - contactPoint.point).normalized;

        // ���Ƃ��Ƃ̈ړ������Ɩ@�������������Ȃ甽�˂��Ȃ�
        if (currentVelocity.IsSameDirection(impactNormal))
        {
            return false;
        }

        Vector2 currentDir = currentVelocity.normalized;
        float currentSpeed = currentVelocity.magnitude;
        // ���˕Ԃ�����̊p�x���Z�o
        Vector2 reflectedDir = Vector2.Reflect(currentDir, impactNormal);

        // �ǂɏՓ˂�����̃X�s�[�h���Z�o
        float newSpeed = currentSpeed * RestitutionCoefficient;

        newVel = reflectedDir * newSpeed;
        return true;
    }

    public enum ECpMoveTrajectoryOnHitWall
    {
        None = 0,// �s���l
        Default, // �ǂɏՓ˂�����ǂɉ����悤�Ɉړ�.���ɉ����������Ȃ��Ƃ����Ȃ�͂�
        Reflect, // ����
    }

    [Inspectable, SerializeField] ECpMoveTrajectoryOnHitWall _trajectory = ECpMoveTrajectoryOnHitWall.None;

    [Inspectable, SerializeField]
    [ShowIf("_trajectory", ECpMoveTrajectoryOnHitWall.Reflect)]
    float RestitutionCoefficient = 1f;
}