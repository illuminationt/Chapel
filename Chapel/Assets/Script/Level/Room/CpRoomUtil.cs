using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Build.Layout;
using UnityEngine;

public static class CpRoomUtil
{
    public static float GridSize => 12f;
    public static ECpRoomConnectAxisType ToAxisType(ECpRoomConnectDirectionType dirType)
    {
        return dirType switch
        {
            ECpRoomConnectDirectionType.Up => ECpRoomConnectAxisType.Vertical,
            ECpRoomConnectDirectionType.Down => ECpRoomConnectAxisType.Vertical,

            ECpRoomConnectDirectionType.Left => ECpRoomConnectAxisType.Horizontal,
            ECpRoomConnectDirectionType.Right => ECpRoomConnectAxisType.Horizontal,

            _ => throw new System.NotImplementedException(),

        };
    }
    public static ECpRoomConnectDirectionType GetInverseDirection(ECpRoomConnectDirectionType dirType)
    {
        return dirType switch
        {

            ECpRoomConnectDirectionType.Up => ECpRoomConnectDirectionType.Down,
            ECpRoomConnectDirectionType.Down => ECpRoomConnectDirectionType.Up,
            ECpRoomConnectDirectionType.Left => ECpRoomConnectDirectionType.Right,
            ECpRoomConnectDirectionType.Right => ECpRoomConnectDirectionType.Left,
            _ => throw new System.NotImplementedException()
        };
    }
    public static Vector2 GetDirection(ECpRoomConnectDirectionType dirType)
    {
        return dirType switch
        {
            ECpRoomConnectDirectionType.Up => -Vector2.up,
            ECpRoomConnectDirectionType.Down => -Vector2.down,
            ECpRoomConnectDirectionType.Left => Vector2.left,
            ECpRoomConnectDirectionType.Right => Vector2.right,
            _ => throw new System.NotImplementedException(),
        };
    }

    public static Vector2Int toVector2Int(in Vector2 vec)
    {
        return new Vector2Int((int)vec.x, (int)vec.y);
    }

    // ���݂̕�������ړ�������̕����̃C���f�N�X���擾
    public static Vector2Int GetNextRoomIndex(in Vector2Int currentIndex, ECpRoomConnectDirectionType dirType)
    {
        Vector2 dirVector = GetDirection(dirType);
        Vector2Int nextRoomIndex = new Vector2Int(currentIndex.x + (int)dirVector.x, currentIndex.y + (int)dirVector.y);
        return nextRoomIndex;
    }

    public static ECpRoomType ToRoomType(ECpRoomUsableType usableType)
    {
        switch (usableType)
        {
            case ECpRoomUsableType.StartPoint: return ECpRoomType.StartPoint;
            case ECpRoomUsableType.Battle: return ECpRoomType.Battle;
            case ECpRoomUsableType.Shop: return ECpRoomType.Shop;
            case ECpRoomUsableType.Boss: return ECpRoomType.Boss;

            case ECpRoomUsableType.PlaceObject:
                Assert.IsTrue(false, "���ƂŎ�������\��");
                return ECpRoomType.None;

            default:
                Assert.IsTrue(false);
                return ECpRoomType.None;
        }
    }
}

public enum ECpRoomType
{
    [InspectorName("�s���l")]
    None = 0,

    [InspectorName("�X�^�[�g�n�_")]
    StartPoint = 1,
    [InspectorName("�퓬����")]
    Battle = 2,
    [InspectorName("�V���b�v")]
    Shop = 3,
    [InspectorName("�{�X��")]
    Boss = 4,
    [InspectorName("�����Ȃ�")]
    Nothing = 5,
}

// �������ǂ̂悤�Ɉ����邩�ݒ�
public enum ECpRoomUsableType
{
    None = -1,
    StartPoint = 0,
    Battle = 10,// �퓬�����͐퓬�����Ƃ��Ă����g���Ȃ�
    PlaceObject = 100,// �I�u�W�F�N�g�z�u�i�V���b�v,�{�X��j
    Shop = 200,// �X
    Boss = 300,// �{�X
}
