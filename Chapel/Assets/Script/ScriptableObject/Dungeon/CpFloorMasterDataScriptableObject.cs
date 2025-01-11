using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1�̃t���A�i�P�K�w�j���\�����邷�ׂĂ̗v�f���܂Ƃ߂��f�[�^
[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpFloorMasterDataScriptableObject")]
public class CpFloorMasterDataScriptableObject : ScriptableObject
{
#if UNITY_EDITOR
    [TextArea]
    public string Comment = null;

    [InfoBox("�P�̊K�w���t���A���\�z����̂ɕK�v�ȃp�����[�^��S�Ă܂Ƃ߂�ScriptableObject.")]

#endif
    public CpRoomProvideParamPerFloor RoomProvideParamPerFloor => _roomProvideSettings.Param;
    // �t���A�̕����v���n�u�����p�����[�^
    [SerializeField]
    [LabelText("Room�̐ݒ���@���uFromProvider�v�ƂȂ��Ă��镔���̋�����")]
    CpRoomProvideParamPerFloorScriptableObject _roomProvideSettings = null;

    public CpFloorStructureParam FloorStructureParam => _floorStructureSettings.Param;
    // �t���A�̍\��
    [SerializeField]
    [LabelText("�t���A�̍\��.��̓I��Room�̔z�u�ꏊ,StartPoint��Boss�����̏ꏊ��.")]
    CpFloorStructureScriptableObject _floorStructureSettings = null;
}
