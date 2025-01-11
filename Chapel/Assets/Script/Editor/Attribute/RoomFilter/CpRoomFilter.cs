using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CpRoomFilterAttribute : PropertyAttribute
{
    public bool Up;
    public bool Down;
    public bool Left;
    public bool Right;

    public CpRoomFilterAttribute(bool up, bool down, bool left, bool right)
    {
        Up = up;
        Down = down;
        Left = left;
        Right = right;
    }
}

[CustomPropertyDrawer(typeof(CpRoomFilterAttribute))]
public class CpRoomFilterDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CpRoomFilterAttribute filterAttribute = (CpRoomFilterAttribute)attribute;

        if (property.propertyType == SerializedPropertyType.ObjectReference)
        {
            EditorGUI.BeginProperty(position, label, property);

            // ���݂̐ݒ�l���擾
            Object currentObject = property.objectReferenceValue;

            // 
            Object selectedObject = EditorGUI.ObjectField(position, label, currentObject, typeof(GameObject), false);

            // �t�B���^�������`�F�b�N
            if (selectedObject is GameObject gameObject)
            {
                if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
                {
                    CpRoom room = gameObject.GetComponent<CpRoom>();
                    TSltBitFlag<ECpRoomConnectDirectionType> roomConnectFlag = room.RoomConnectFlag;


                }
            }
        }
    }
}