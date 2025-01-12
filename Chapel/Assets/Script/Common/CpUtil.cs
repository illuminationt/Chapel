using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * ���[���h���W,�X�N���[�����W,���K�����W�ɂ���
 * 
 * �����[���h���W(WorldPosition):���ۂɃI�u�W�F�N�g���z�u���Ă����Ԃ̍��W�B
 * 
 * ���X�N���[�����W(ScreenPosition):������(0,0)�Ƃ��A1px���Ƃɍ��W��1�傫���Ȃ��ԁB
 * �Ⴆ�΁A(0,0)����E��500px,���300px�i�񂾂Ƃ���̃X�N���[�����W��(500,300)
 * 
 * �����K�����W(NormalizedPosition)
 * ��ʂ̍�����(0,0),�E���(1,1)�Ƃ�����W�n
 * 
 */
public static class CpUtil
{
    public static Vector2 GetWorldPositionFromNormalizedPosition(in Vector2 normalizedPosition)
    {
        Vector2 screenPoint = new Vector2(normalizedPosition.x, normalizedPosition.y);
        Camera mainCamera = Camera.main;
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(screenPoint);
        return worldPosition;
    }

    public static Vector2 GetNormalizedPositionFromWorldPosition(in Vector2 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(worldPosition);
        return screenPosition;
    }

    public static Vector2 GetWorldPositionFromScreenPosition(in Vector2 screenPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static Vector2 GetNormalizedPositionFromScreenPosition(in Vector2 screenPosition)
    {
        Vector2 worldPosition = GetWorldPositionFromScreenPosition(screenPosition);
        Vector2 normalizedPosition = GetNormalizedPositionFromWorldPosition(worldPosition);
        return normalizedPosition;
    }

    public static bool IsInScreen(in Vector2 worldPosition, float boudingRadius)
    {
        Vector2 np = GetNormalizedPositionFromWorldPosition(worldPosition);
        return 0f <= np.x && np.x <= 1f && 0 <= np.y && np.y <= 1f;
    }

    public static Vector2 CalcDeltaVector(Vector2 offsetVector, ECpOffsetType offsetType)
    {
        switch (offsetType)
        {
            case ECpOffsetType.Absolute:
                return offsetVector;
            case ECpOffsetType.Screen:
                offsetVector.x *= CpScreen.Width;
                offsetVector.y *= CpScreen.Height;
                return offsetVector;
            default:
                Assert.IsTrue(false);
                return Vector2.zero;
        }
    }

    public static Vector2 GetPlayerWorldPosition()
    {
        CpPlayer player = CpGameManager.Instance.Player;
        Vector2 pos = player.transform.position;
        return pos;
    }

    public static Vector2 GetDirectionToPlayer(in Vector2 origin)
    {
        Vector2 playerPos = GetPlayerWorldPosition();
        return (playerPos - origin).normalized;
    }

}
