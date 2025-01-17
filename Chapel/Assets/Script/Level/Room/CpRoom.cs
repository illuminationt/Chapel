using ImGuiNET;
using NUnit.Framework;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public enum ECpRoomState
{
    Open,
    Closed,
}

public class CpRoom : CpActorBase
{
    enum ECpRoomColliderType
    {
        None,
        Wall,
        Gate,
        TransitionTrigger,
    }

    public static CpRoom CreateRoomInstance(CpRoom prefab, CpRoomProxy ownerRoomProxy)
    {
        var instance = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        instance.Initialize(ownerRoomProxy);
        return instance;
    }
    void Initialize(CpRoomProxy ownerRoomProxy)
    {
        _ownerRoomProxy = ownerRoomProxy;

        bool bGateOpened = _ownerRoomProxy.GetRoomFlag(ECpRoomFlags.GateOpens);
        OpenGate(bGateOpened);
    }

    Tilemap _tilemap = null;
    protected override void Awake()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
        Assert.IsTrue(_tilemap != null);
    }

    public TSltBitFlag<ECpRoomConnectDirectionType> RoomConnectFlag
    {
        get
        {
            TSltBitFlag<ECpRoomConnectDirectionType> retFlag = default;
            retFlag.Set(ECpRoomConnectDirectionType.Up, _gateUpObject.gameObject.activeSelf);
            retFlag.Set(ECpRoomConnectDirectionType.Down, _gateDownObject.gameObject.activeSelf);
            retFlag.Set(ECpRoomConnectDirectionType.Left, _gateLeftObject.gameObject.activeSelf);
            retFlag.Set(ECpRoomConnectDirectionType.Right, _gateRightObject.gameObject.activeSelf);
            return retFlag;
        }
    }

    public void OpenGate(bool bOpen)
    {
        VisitGates((CpRoomGate gate) =>
        {
            gate.Open(bOpen);
        });
    }

    // �����ɓ������Ƃ��̏���������
    public void OnEnterRoom()
    {
        SetEnableCollision(ECpRoomColliderType.Wall, true);
        SetEnableCollision(ECpRoomColliderType.TransitionTrigger, true);
    }

    // ��������o�����̏���
    public void OnExitRoom()
    {
        SetEnableCollision(ECpRoomColliderType.Wall, false);
        SetEnableCollision(ECpRoomColliderType.Gate, false);
        SetEnableCollision(ECpRoomColliderType.TransitionTrigger, false);
    }

    void SetEnableCollision(ECpRoomColliderType colliderType, bool bEnable)
    {
        List<Collider2D> colliders = GetColliders(colliderType);
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = bEnable;
        }
    }

    List<Collider2D> GetColliders(ECpRoomColliderType colliderType)
    {
        switch (colliderType)
        {
            case ECpRoomColliderType.Wall:
                return _wallTilemapColliders;
            case ECpRoomColliderType.Gate:
                return _gateColliders;
            case ECpRoomColliderType.TransitionTrigger:
                return transitionTriggers;
            default:
                Assert.IsTrue(false);
                return null;
        }
    }

    // public const �֐��Q

    // �Q�[�g�I�u�W�F�N�g�́A���[�g����̑��΍��W���擾
    public Vector2 GetGateRelativeLocation(ECpRoomConnectDirectionType dirType)
    {
        CpRoomGate gate = GetGate(dirType);
        return gate.transform.localPosition;
    }

    // �������ړ�����Ƃ��́A�ړ��O�̕����̈ړ��ʂ��擾
    public Vector2 GetTranslationOnTransition(CpRoom nextRoom, ECpRoomConnectDirectionType dirType)
    {
        Vector2 prevRoomGatePosition = GetGateRelativeLocation(dirType);

        ECpRoomConnectDirectionType nextRoomGateDirType = CpRoomUtil.GetInverseDirection(dirType);
        Vector2 nextRoomGatePosition = nextRoom.GetGateRelativeLocation(nextRoomGateDirType);

        // �ړ��O�̕����̃Q�[�g���W���A�ړ���̕����̃Q�[�g���W�܂Ŏ����Ă���K�v������
        Vector2 gateTranslation = nextRoomGatePosition - prevRoomGatePosition;

        // gateTranslation�����ړ�������ƁA�Q�[�g���W����v������ԂɂȂ�B
        // �O���b�h�̃}�X�P���������炷�K�v������
        ECpRoomConnectAxisType transitionAxis = CpRoomUtil.ToAxisType(dirType);
        switch (transitionAxis)
        {
            case ECpRoomConnectAxisType.Horizontal:
                gateTranslation.x += Mathf.Sign(gateTranslation.x) * CpRoomUtil.GridSize;
                break;
            case ECpRoomConnectAxisType.Vertical:
                gateTranslation.y += Mathf.Sign(gateTranslation.y) * CpRoomUtil.GridSize;
                break;
            default:
                Assert.IsTrue(false);
                break;
        }

        return gateTranslation;
    }

    // �����ɓ������Ƃ��̎��@���W���擾
    public Vector2 GetPlayerPositionOnEnterRoom(CpRoom nextRoom, ECpRoomConnectDirectionType exitDirType)
    {
        CpPlayer player = CpPlayer.Get();


        // �ړ���̕����̃Q�[�g���W���擾
        ECpRoomConnectDirectionType nextRoomEnterDir = CpRoomUtil.GetInverseDirection(exitDirType);
        Vector2 nextRoomEnterLocation = nextRoom.GetGateRelativeLocation(nextRoomEnterDir);

        // �����ɓ���Ƃ��̓Q�[�g���W�����������i�񂾏ꏊ���������W�ɂ���K�v������
        Vector2 enter1gridOffset = exitDirType switch
        {
            ECpRoomConnectDirectionType.Up => Vector2.up * CpRoomUtil.GridSize,
            ECpRoomConnectDirectionType.Down => Vector2.down * CpRoomUtil.GridSize,
            ECpRoomConnectDirectionType.Left => Vector2.left * CpRoomUtil.GridSize,
            ECpRoomConnectDirectionType.Right => Vector2.right * CpRoomUtil.GridSize,
            _ => throw new System.NotImplementedException(),
        };

        // �㉺�ړ��Ȃ獶�E����;���E�����Ȃ�㉺���� �̃I�t�Z�b�g�����킹��
        // ��F�Q�[�g�̍��[����o���Ȃ�A�Q�[�g�̉E�[�������
        ECpRoomConnectAxisType exitAxisType = CpRoomUtil.ToAxisType(exitDirType);
        // �������o���u�Ԃ́A���@�ƃQ�[�g�̍��W�������擾
        Vector2 gateLocation = GetGateRelativeLocation(exitDirType);
        Vector2 playerLocation = player.transform.position;
        Vector2 playerGateOffset = playerLocation - gateLocation;
        Vector2 playerNextGateOffset = exitAxisType switch
        {
            ECpRoomConnectAxisType.Vertical => Vector2.right * playerGateOffset.x,
            ECpRoomConnectAxisType.Horizontal => Vector2.up * playerGateOffset.y,
            _ => throw new System.NotImplementedException()
        };

        Vector2 retPosition = nextRoomEnterLocation + enter1gridOffset + playerNextGateOffset;
        return retPosition;
    }

    public bool IsConnectFlagMatch(bool bUp, bool bDown, bool bLeft, bool bRight)
    {
        return RoomConnectFlag.EqualsFlag(ECpRoomConnectDirectionType.Up, bUp) &&
               RoomConnectFlag.EqualsFlag(ECpRoomConnectDirectionType.Down, bDown) &&
               RoomConnectFlag.EqualsFlag(ECpRoomConnectDirectionType.Left, bLeft) &&
               RoomConnectFlag.EqualsFlag(ECpRoomConnectDirectionType.Right, bRight);
    }

    public CpRoomUsableParam GetRoomUsableParam() => _roomUsableParam;

    // private �֐��Q

    void VisitGates(UnityAction<CpRoomGate> Func)
    {
        Func(_gateUpObject);
        Func(_gateDownObject);
        Func(_gateLeftObject);
        Func(_gateRightObject);
    }

    CpRoomGate GetGate(ECpRoomConnectDirectionType dirType)
    {
        return dirType switch
        {
            ECpRoomConnectDirectionType.Up => _gateUpObject,
            ECpRoomConnectDirectionType.Down => _gateDownObject,
            ECpRoomConnectDirectionType.Left => _gateLeftObject,
            ECpRoomConnectDirectionType.Right => _gateRightObject,
            _ => throw new System.NotImplementedException(),
        };
    }

    public ECpRoomType RoomType
    {
        get
        {
            Assert.IsTrue(!SltUtil.IsPrefab(this));
            return _ownerRoomProxy == null ? ECpRoomType.None : _ownerRoomProxy.GetRoomType();
        }
    }

    // ���̃A�Z�b�g���g�p�\�ȃ_���W�������ʗpID
    //�@�_���W����A��UsableId=1,3���g��...�݂�����
    // �d�l���l���ĂȂ��̂ł����܂��ȕ����g�p���邱�ƂɂȂ��Ă���,�ł���Ες���������
    public int UsableId = -1;
    public ECpRoomUsableType RoomUsableType = ECpRoomUsableType.None;
    [SerializeField][FoldoutGroup("Ref_Gate")] CpRoomGate _gateUpObject = null;
    [SerializeField][FoldoutGroup("Ref_Gate")] CpRoomGate _gateDownObject = null;
    [SerializeField][FoldoutGroup("Ref_Gate")] CpRoomGate _gateLeftObject = null;
    [SerializeField][FoldoutGroup("Ref_Gate")] CpRoomGate _gateRightObject = null;
    [SerializeField][FoldoutGroup("Ref_Collider")] List<Collider2D> _wallTilemapColliders = null;
    [SerializeField][FoldoutGroup("Ref_Collider")] List<Collider2D> _gateColliders = null;
    [SerializeField][FoldoutGroup("Ref_Collider")] List<Collider2D> transitionTriggers = null;

    [SerializeField]
    CpRoomUsableParam _roomUsableParam = null;

    CpRoomProxy _ownerRoomProxy = null;

#if UNITY_EDITOR
    CpRoomGate FindGate(ECpRoomConnectDirectionType dirType)
    {
        CpRoomGate[] gates = GetComponentsInChildren<CpRoomGate>(true);
        foreach (CpRoomGate gate in gates)
        {
            CpRoomTransitionTrigger childTrigger = gate.GetComponentInChildren<CpRoomTransitionTrigger>(true);
            ECpRoomConnectDirectionType triggerDirType = childTrigger.ConnectDirection;

            if (triggerDirType == dirType)
            {
                return gate;
            }
        }
        return null;
    }
    GameObject FindWallTileMap(ECpRoomConnectDirectionType dirType)
    {
        Tilemap[] tileMaps = GetComponentsInChildren<Tilemap>(true);
        foreach (Tilemap tilemap in tileMaps)
        {
            string objName = tilemap.gameObject.name;
            string searchStr = dirType switch
            {

                ECpRoomConnectDirectionType.Up => "_Up",
                ECpRoomConnectDirectionType.Down => "_Down",
                ECpRoomConnectDirectionType.Left => "_Left",
                ECpRoomConnectDirectionType.Right => "_Right",
                _ => throw new System.NotImplementedException(),
            };
            if (objName.Contains(searchStr))
            {
                return tilemap.gameObject;
            }
        }
        return null;
    }
    [Button("�Q�[�g�������ݒ�")]
    void SetGates()
    {
        // �e�q�\���͈ȉ��̒ʂ�
        /**
         * CpRoom->CpRoomGate->RoomTransitionTrigger
         */

        _gateUpObject = FindGate(ECpRoomConnectDirectionType.Up);
        _gateDownObject = FindGate(ECpRoomConnectDirectionType.Down);
        _gateLeftObject = FindGate(ECpRoomConnectDirectionType.Left);
        _gateRightObject = FindGate(ECpRoomConnectDirectionType.Right);

    }

    [LabelText("�Q�[�gor�ǐ؂�ւ�")]
    [HorizontalGroup("ToggleButton")]
    [Button("��")]
    void ToggleUp() { ToggleGateOrWall(ECpRoomConnectDirectionType.Up); }
    [HorizontalGroup("ToggleButton")]
    [Button("��")]
    void ToggleDown() { ToggleGateOrWall(ECpRoomConnectDirectionType.Down); }

    [HorizontalGroup("ToggleButton")]
    [Button("��")]
    void ToggleRight() { ToggleGateOrWall(ECpRoomConnectDirectionType.Right); }
    [HorizontalGroup("ToggleButton")]
    [Button("��")]
    void ToggleLeft() { ToggleGateOrWall(ECpRoomConnectDirectionType.Left); }

    CpRoomGate FindGate(string objName)
    {
        if (objName.Contains("Up")) { return _gateUpObject; }
        if (objName.Contains("Down")) { return _gateDownObject; }
        if (objName.Contains("Left")) { return _gateLeftObject; }
        if (objName.Contains("Right")) { return _gateRightObject; }
        return null;
    }

    void ToggleGateOrWall(ECpRoomConnectDirectionType dirType)
    {
        CpRoomGate gate = FindGate(dirType);
        bool bNewGateActive = !gate.gameObject.activeInHierarchy;
        gate.gameObject.SetActive(bNewGateActive);

        GameObject wallTilemap = FindWallTileMap(dirType);
        wallTilemap.SetActive(!bNewGateActive);
    }

    private void OnDrawGizmos()
    {
        Vector2 arrow1end = new Vector2(50f, 0f);
        SltDebugDrawOnGizmos.DrawArrow(Vector2.zero, arrow1end, Color.red);

        Vector2 arrow2end = new Vector2(0f, 50f);
        SltDebugDrawOnGizmos.DrawArrow(Vector2.zero, arrow2end, Color.green);
    }

    public void DrawImGui()
    {
        bool bActive = gameObject.activeInHierarchy;
        if (ImGui.Checkbox("Active", ref bActive))
        {
            gameObject.SetActive(bActive);
        }

        if (ImGui.TreeNode("Gates"))
        {
            VisitGates((CpRoomGate gate) =>
            {
                string treeTitle = gate.name;
                if (ImGui.TreeNode(treeTitle))
                {
                    gate.DrawImGui();
                    ImGui.TreePop();
                }
            });
            ImGui.TreePop();
        }

    }
#endif
}