using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using DG.DemiEditor;
using NUnit.Framework;




#if UNITY_EDITOR
using UnityEditor;

#endif


public class CpFloorEditorWindow : EditorWindow
{
    CpFloorStructureScriptableObject _floorMasterDataSO = null;
    public static CpFloorStructureRoomParam EditingRoomParam = null;
    int _workingHeight = -1;
    int _workingWidth = -1;
    TSltBitFlag<KeyCode> _pressedKeyCodeFlags;
    readonly int roomButtonSize = 80;
    [MenuItem("Chapel/Floor Editor")]
    public static void ShowWindow()
    {
        GetWindow<CpFloorEditorWindow>("Chapel Floor Editor");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Chapel Floor Editor", EditorStyles.boldLabel);

        _floorMasterDataSO = (CpFloorStructureScriptableObject)EditorGUILayout.ObjectField("Floor Master Data", _floorMasterDataSO, typeof(CpFloorStructureScriptableObject), false);

        if (_floorMasterDataSO == null)
        {
            EditorGUILayout.HelpBox("Floor Master Dataを選択して下さい", MessageType.Warning);
            return;
        }

        // キー押下監視
        Event e = Event.current;
        if (e.type == EventType.KeyUp)
        {
            _pressedKeyCodeFlags.Clear();
        }
        if (e.type == EventType.KeyDown)
        {
            _pressedKeyCodeFlags.Set(e.keyCode, true);
        }

        CpFloorStructureParam currentParam = _floorMasterDataSO.Param;

        _workingHeight = EditorGUILayout.IntField("行の数", _workingHeight < 0 ? currentParam.RoomHeight : _workingHeight);
        _workingWidth = EditorGUILayout.IntField("列の数", _workingWidth < 0 ? currentParam.RoomHeight : _workingWidth);

        DrawTranslate();
        DrawDescription();

        if (GUILayout.Button("保存"))
        {
            currentParam.SetDungeonSize(_workingWidth, _workingHeight);
            EditorUtility.SetDirty(_floorMasterDataSO);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        for (int y = 0; y < _workingHeight; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < _workingWidth; x++)
            {
                CpFloorStructureRoomParam roomParam = currentParam.FindRoomParam(x, y);
                bool bParamValid = IsValidParam(roomParam);
                string buttonString = bParamValid ? GetRoomButtonString(x, y, roomParam) : strError;
                GUIStyle style = GetButtonStyle(roomParam);
                if (GUILayout.Button(buttonString, style, GUILayout.Width(roomButtonSize), GUILayout.Height(roomButtonSize)))
                {
                    EditRoom(roomParam);
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    void EditRoom(CpFloorStructureRoomParam roomParam)
    {
        // 同時押し判定
        if (OnRoomPressed(roomParam))
        {
            return;
        }
        EditingRoomParam = roomParam;
        if (!HasOpenInstances<CpFloorRoomParamEditorWindow>())
        {
            CreateWindow<CpFloorRoomParamEditorWindow>("Floor Room Param");
        }
    }

    static readonly string strError = "ERROR";
    static readonly string strEmpty = "　　　";
    static readonly string strUp = "　↑　";
    static readonly string strLeft = "←　";
    static readonly string strRight = "　　→";
    static readonly string strLeftRight = "←　→";
    static readonly string strDown = "　↓　";

    bool IsValidParam(CpFloorStructureRoomParam roomParam)
    {
        if (roomParam == null)
        {
            return false;
        }

        switch (roomParam.RoomSelectType)
        {
            case ECpRoomSelectType.NoRoom:
                return true;
            case ECpRoomSelectType.Fixed:
                {
                    CpRoom roomPrefab = roomParam.FixedRoomParam.GetRoomPrefab();
                    if (roomPrefab == null)
                    {
                        return false;
                    }
                    return true;
                }
            case ECpRoomSelectType.FromProvider:
                {
                    return roomParam.ProvideParam.IsValid();
                }

            default:
                throw new System.NotImplementedException();

        }
    }

    // ボタンに表示する文字列を取得
    string GetRoomButtonString(int x, int y, CpFloorStructureRoomParam roomParam)
    {
        string retString = "";
        retString = GetCoordinateString(x, y) + "\n";
        retString += GetRoomConnectString(roomParam);

        ECpRoomUsableType roomUsableType = roomParam.GetRoomUsableType();
        retString += SltEnumUtil.ToString(roomUsableType) + "\n";

        return retString;
    }

    string GetCoordinateString(int x, int y)
    {
        string ret = $"({x},{y})";
        return ret;
    }

    string GetRoomConnectString(CpFloorStructureRoomParam roomParam)
    {
        if (roomParam == null)
        {
            return strEmpty;
        }
        switch (roomParam.RoomSelectType)
        {
            case ECpRoomSelectType.NoRoom:
                return strEmpty;
            case ECpRoomSelectType.Fixed:
                return GetRoomConnectString(roomParam.FixedRoomParam.GetRoomPrefab().RoomConnectFlag);
            case ECpRoomSelectType.FromProvider:
                return GetRoomConnectString(roomParam.ProvideParam.ConnectionFlag);
            default:
                throw new System.NotImplementedException();
        }
    }

    GUIStyle GetButtonStyle(CpFloorStructureRoomParam roomParam)
    {
        GUIStyle style = new GUIStyle(GUI.skin.button);
        if (roomParam == null) { return style; }

        Color backgroundColor = roomParam.RoomSelectType switch
        {
            ECpRoomSelectType.NoRoom => Color.gray,
            ECpRoomSelectType.Fixed => Color.white,
            ECpRoomSelectType.FromProvider => Color.green,
            _ => throw new System.NotImplementedException(),
        };
        if (!IsValidParam(roomParam))
        {
            backgroundColor = Color.red;
        }
        Color textColor = roomParam.RoomSelectType switch
        {

            ECpRoomSelectType.NoRoom => Color.white,
            ECpRoomSelectType.Fixed => Color.black,
            ECpRoomSelectType.FromProvider => Color.black,
            _ => throw new System.NotImplementedException()
        };
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, backgroundColor);
        texture.Apply();
        style.normal.background = texture;

        style.normal.textColor = textColor;
        return style;
    }

    // 部屋同士の連結を表す文字列取得
    string GetRoomConnectString(TSltBitFlag<ECpRoomConnectDirectionType> roomConnectFlag)
    {
        string retString = "";

        // １行目
        retString += roomConnectFlag.Get(ECpRoomConnectDirectionType.Up) ? strUp : strEmpty;
        retString += "\n";

        // ２行目
        bool bLeft = roomConnectFlag.Get(ECpRoomConnectDirectionType.Left);
        bool bRight = roomConnectFlag.Get(ECpRoomConnectDirectionType.Right);
        if (bLeft && bRight)
        {
            retString += strLeftRight;
        }
        else if (bLeft)
        {
            retString += strLeft;
        }
        else if (bRight)
        {
            retString += strRight;
        }
        else
        {
            retString += strEmpty;
        }
        retString += "\n";

        // ３行目
        retString += roomConnectFlag.Get(ECpRoomConnectDirectionType.Down) ? strDown : strEmpty;
        retString += "\n";

        return retString;
    }

    void DrawTranslate()
    {
        Vector2Int deltaMove = Vector2Int.zero;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("↑", GUILayout.Width(25)))
        {
            deltaMove.y--;
        }
        if (GUILayout.Button("→", GUILayout.Width(25)))
        {
            deltaMove.x++;
        }
        if (GUILayout.Button("↓", GUILayout.Width(25)))
        {
            deltaMove.y++;
        }
        if (GUILayout.Button("←", GUILayout.Width(25)))
        {
            deltaMove.x--;
        }
        GUILayout.EndHorizontal();
        Translate(deltaMove.x, deltaMove.y);

    }
    void Translate(int deltaX, int deltaY)
    {
        if (deltaX != 0 || deltaY != 0)
        {
            _floorMasterDataSO.Param.Translate(deltaX, deltaY);
        }
    }
    // ショートカット操作を説明
    void DrawDescription()
    {
        EditorGUILayout.LabelField("部屋選択方法... 1:No Room 2:Fixed 3:FromProvider");
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("From Provider Shortcuts");
        EditorGUILayout.LabelField("WASD:遷移可能方向トグル");
        EditorGUILayout.LabelField("E:部屋の種類順繰り設定");
    }
    // 部屋をクリックするときに同時押ししてたキーに応じて処理を分ける
    bool OnRoomPressed(CpFloorStructureRoomParam roomParam)
    {
        if (_pressedKeyCodeFlags.Get(KeyCode.Alpha1))
        {
            SetRoomSelectType(roomParam, 1);
            return true;
        }
        if (_pressedKeyCodeFlags.Get(KeyCode.Alpha2))
        {
            SetRoomSelectType(roomParam, 2);
            return true;
        }
        if (_pressedKeyCodeFlags.Get(KeyCode.Alpha3))
        {
            SetRoomSelectType(roomParam, 3);
            return true;
        }
        if (_pressedKeyCodeFlags.Get(KeyCode.W))
        {
            ToggleConnectionFlag(roomParam, ECpRoomConnectDirectionType.Up);
            return true;
        }
        if (_pressedKeyCodeFlags.Get(KeyCode.D))
        {
            ToggleConnectionFlag(roomParam, ECpRoomConnectDirectionType.Right);
            return true;
        }
        if (_pressedKeyCodeFlags.Get(KeyCode.S))
        {
            ToggleConnectionFlag(roomParam, ECpRoomConnectDirectionType.Down);
            return true;
        }
        if (_pressedKeyCodeFlags.Get(KeyCode.A))
        {
            ToggleConnectionFlag(roomParam, ECpRoomConnectDirectionType.Left);
            return true;
        }
        if (_pressedKeyCodeFlags.Get(KeyCode.Q))
        {
            ToggleConnectionFlag(roomParam, ECpRoomConnectDirectionType.Up);
            ToggleConnectionFlag(roomParam, ECpRoomConnectDirectionType.Up);
            return true;
        }

        if (_pressedKeyCodeFlags.Get(KeyCode.E))
        {
            ShiftRoomType(roomParam);
            return true;
        }

        return false;
    }

    void SetRoomSelectType(CpFloorStructureRoomParam roomParam, int index)
    {
        List<ECpRoomSelectType> roomSelectTypes = SltEnumUtil.GetAllValues<ECpRoomSelectType>();
        roomParam.RoomSelectType = roomSelectTypes[index - 1];
    }

    void ToggleConnectionFlag(CpFloorStructureRoomParam roomParam, ECpRoomConnectDirectionType dirType)
    {
        roomParam.RoomSelectType = ECpRoomSelectType.FromProvider;
        bool bFlag = roomParam.ProvideParam.ConnectionFlag.Get(dirType);
        roomParam.ProvideParam.ConnectionFlag.Set(dirType, !bFlag);
    }
    void ShiftRoomType(CpFloorStructureRoomParam roomParam)
    {
        CpRoomRequestParam provideParam = roomParam.ProvideParam;
        ECpRoomUsableType nextRoomUsableType = SltEnumUtil.GetNextEnumValue(provideParam.RoomUsableType);
        roomParam.ProvideParam.RoomUsableType = nextRoomUsableType;
    }
}

public class CpFloorRoomParamEditorWindow : EditorWindow
{
    private void OnGUI()
    {
        CpFloorStructureRoomParam activeRoomParam = CpFloorEditorWindow.EditingRoomParam;
        if (activeRoomParam == null)
        {
            EditorGUILayout.LabelField("部屋が選択されていません");
            return;
        }
        activeRoomParam.RoomSelectType = (ECpRoomSelectType)EditorGUILayout.EnumPopup("部屋選択方法", activeRoomParam.RoomSelectType);

        switch (activeRoomParam.RoomSelectType)
        {
            case ECpRoomSelectType.NoRoom:
                DrawRoomSelectTypeNoRoom();
                break;

            case ECpRoomSelectType.Fixed:
                DrawRoomFixed(activeRoomParam.FixedRoomParam);
                break;

            case ECpRoomSelectType.FromProvider:
                DrawRoomFromProvider(activeRoomParam.ProvideParam);
                break;

            default:
                throw new System.NotImplementedException();
        }
    }

    void DrawRoomSelectTypeNoRoom()
    {

    }
    void DrawRoomFixed(CpFixedRoomParam fixedParam)
    {
        CpRoom roomPrefab = (CpRoom)EditorGUILayout.ObjectField(
            fixedParam.GetRoomPrefab(),
            typeof(CpRoom), false);
        fixedParam.DebugSetRoomPrefab(roomPrefab);


        EditorGUILayout.LabelField("部屋使用時パラメータオーバーライドフラグ");
        CpRoomUsableParamOverride paramOverride = fixedParam.DebugGetUsableParamOverride;
        paramOverride.DebugIsOverride = EditorGUILayout.Toggle(paramOverride.DebugIsOverride);

        if (paramOverride.DebugIsOverride)
        {
            ECpRoomUsableType roomUsableType = fixedParam.GetRoomUsableType();

            switch (roomUsableType)
            {
                case ECpRoomUsableType.StartPoint:
                    DrawRoomFixed_StartPoint();
                    Assert.IsTrue(false);
                    break;
                case ECpRoomUsableType.Battle:
                    DrawRoomFixed_Battle(paramOverride.DebugGetRoomUsableParam.ParamBattle);
                    break;
                case ECpRoomUsableType.PlaceObject:
                    Assert.IsTrue(false);
                    break;
                case ECpRoomUsableType.Shop:
                    DrawRoomFixed_Shop(paramOverride.DebugGetRoomUsableParam.ParamShop);
                    break;
                case ECpRoomUsableType.Boss:
                    DrawRoomFixed_Boss(paramOverride.DebugGetRoomUsableParam.ParamBoss);
                    break;
            }
        }
    }
    void DrawRoomFixed_StartPoint()
    {
        // 未実装
    }
    void DrawRoomFixed_Battle(CpRoomUsableParamBattle paramBattle)
    {
        paramBattle.ParamSelectType = (ECpEnemySpawnParamSelectType)EditorGUILayout.EnumPopup("エネミー生成パラメータ生成方法", paramBattle.ParamSelectType);

        if (paramBattle.ParamSelectType == ECpEnemySpawnParamSelectType.FixedData)
        {

            paramBattle.SpawnParamScriptableObject = (CpEnemySpawnParamScriptableObject)EditorGUILayout.ObjectField("エネミー生成用ScriptableObject", paramBattle.SpawnParamScriptableObject, typeof(CpEnemySpawnParamScriptableObject), false);
        }
    }
    void DrawRoomFixed_PlaceObject()
    {
    }
    void DrawRoomFixed_Shop(CpRoomUsableParamShop paramShop)
    {
    }
    void DrawRoomFixed_Boss(CpRoomUsableParamBoss paramBoss)
    {

    }
    void DrawRoomFromProvider(CpRoomRequestParam provideParam)
    {
        DrawDirectionFlag(ref provideParam.ConnectionFlag);

        provideParam.RoomUsableType = (ECpRoomUsableType)EditorGUILayout.EnumPopup("部屋の種類", provideParam.RoomUsableType);


    }



    void DrawDirectionFlag(ref TSltBitFlag<ECpRoomConnectDirectionType> connectionFlag)
    {
        DrawDirectionFlagInternal("↑", ref connectionFlag, ECpRoomConnectDirectionType.Up);
        DrawDirectionFlagInternal("→", ref connectionFlag, ECpRoomConnectDirectionType.Left);
        DrawDirectionFlagInternal("↓", ref connectionFlag, ECpRoomConnectDirectionType.Down);
        DrawDirectionFlagInternal("←", ref connectionFlag, ECpRoomConnectDirectionType.Right);
    }

    void DrawDirectionFlagInternal(string title, ref TSltBitFlag<ECpRoomConnectDirectionType> connectionFlag, ECpRoomConnectDirectionType dirType)
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(title);

        bool bFlag = connectionFlag.Get(dirType);
        bFlag = EditorGUILayout.Toggle(bFlag);
        connectionFlag.Set(dirType, bFlag);

        EditorGUILayout.EndHorizontal();
    }
}