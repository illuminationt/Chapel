using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;

public class CpRoomProxyManager
{
    public static CpRoomProxyManager Get()
    {
        CpDungeonManager dungeonMan = CpDungeonManager.Get();
        return dungeonMan.RoomProxyManager;
    }

    public void Initialize(CpFloorMasterDataScriptableObject floorMasterSettings)
    {
        _roomProxies = null;

        CpFloorStructureParam floorStructureParam = floorMasterSettings.FloorStructureParam;
        CpRoomProvideParamPerFloor roomProvideParam = floorMasterSettings.RoomProvideParamPerFloor;

        // あらかじめメモリ確保しておく
        int height = floorStructureParam.RoomHeight;
        int width = floorStructureParam.RoomHeight;

        _roomProxies = new List<List<CpRoomProxy>>(height);
        for (int y = 0; y < height; y++)
        {
            List<CpRoomProxy> proxyList = new List<CpRoomProxy>(width);

            for (int x = 0; x < width; x++)
            {
                CpFloorStructureRoomParam roomParam = floorStructureParam.FindRoomParam(x, y);
                CpRoomProxy newProxy = CpRoomProxy.Create(roomParam, roomProvideParam, new Vector2Int(x, y));
                proxyList.Add(newProxy);
            }
            _roomProxies.Add(proxyList);
        }

        // RoomUsableTypeを
    }
    public CpRoomProxy FindRoomProxy(int x, int y)
    {
        if (!ExistsRoom(x, y))
        {
            return null;
        }

        return _roomProxies[y][x];
    }
    public CpRoomProxy FindRoomProxy(in Vector2Int index)
    {
        return FindRoomProxy(index.x, index.y);
    }
    public CpRoomProxy FindRoomProxy(CpRoomProxyId id)
    {
        for (int y = 0; y < _roomProxies.Count; y++)
        {
            for (int x = 0; x < _roomProxies[y].Count; x++)
            {
                CpRoomProxy proxy = FindRoomProxy(x, y);
                if (proxy != null)
                {
                    if (proxy.IsMatchId(id))
                    {
                        return proxy;
                    }
                }
            }
        }
        return null;
    }
    public CpRoomProxy GetActiveRoomProxy()
    {
        for (int y = 0; y < _roomProxies.Count; y++)
        {
            for (int x = 0; x < _roomProxies[y].Count; x++)
            {
                CpRoomProxy proxy = FindRoomProxy(x, y);
                if (proxy != null)
                {
                    if (proxy.GetRoomFlag(ECpRoomFlags.IsPlayerIn))
                    {
                        return proxy;
                    }
                }
            }
        }
        return null;
    }

    bool ExistsRoom(int x, int y)
    {
        if (!_roomProxies.IsValidIndex(y))
        {
            return false;
        }
        if (!_roomProxies[y].IsValidIndex(x))
        {
            return false;
        }
        return true;

    }

    List<List<CpRoomProxy>> _roomProxies = new List<List<CpRoomProxy>>();

#if CP_DEBUG
    public void DrawImGui()
    {
        DrawRoomProxies();
    }

    void DrawRoomProxies()
    {
        if (_roomProxies == null)
        {
            ImGui.Text("今は部屋が存在していません"); return;
        }
        int yNum = _roomProxies.Count;
        int xNum = _roomProxies[0].Count; ;
        if (ImGui.BeginTable("MyTable", xNum)) // カラム数を指定
        {
            // カラムヘッダー
            ImGui.TableNextRow();
            for (int xindex = 0; xindex < xNum; xindex++)
            {
                ImGui.TableSetColumnIndex(xindex);
                ImGui.Text($"X={xindex}");
            }

            // データ行
            for (int y = 0; y < yNum; y++)
            {
                ImGui.TableNextRow();
                for (int x = 0; x < xNum; x++)
                {
                    ImGui.TableSetColumnIndex(x);

                    CpRoomProxy roomProxy = FindRoomProxy(x, y);
                    if (roomProxy != null)
                    {
                        bool bPlayerIn = roomProxy.GetRoomFlag(ECpRoomFlags.IsPlayerIn);
                        if (bPlayerIn)
                        {
                            ImGui.Text("(*)");
                            ImGui.SameLine();
                        }
                    }

                    if (ImGui.Button($"({x},{y})"))
                    {
                        selectingRoomIndex.x = x;
                        selectingRoomIndex.y = y;
                    }
                }
            }

            ImGui.EndTable();
        }

        CpRoomProxy selectingRoomProxy = FindRoomProxy(selectingRoomIndex);
        if (selectingRoomProxy == null)
        {
            ImGui.Text("選択中の部屋プロキシは存在しません");
        }
        else
        {
            selectingRoomProxy.DrawImGui();
        }
    }
    Vector2Int selectingRoomIndex = new Vector2Int(-1, -1);
#endif
}
