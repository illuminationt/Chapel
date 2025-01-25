using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * �t���A�̕����̔z�u�ꏊ�A�������m�̌q����̃f�[�^
 */
[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpFloorStructureScriptableObject")]
public class CpFloorStructureScriptableObject : ScriptableObject
{
#if CP_EDITOR
    [TextArea]
    public string Comment = null;

#endif

    public CpFloorStructureParam Param => _floorStructureParam;

    // �t���A�̍\��
    [SerializeField]
    [ReadOnly]
    CpFloorStructureParam _floorStructureParam;
}
