using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CpDrawRenderTextureParam
{
    public void Draw(in Rect Rect)
    {
        if (Enable)
        {
            GUI.DrawTexture(Rect, RenderTexture, ScaleMode, AlphaBlend);
        }
    }
    public RenderTexture RenderTexture;
    public ScaleMode ScaleMode = ScaleMode.ScaleToFit;
    public bool AlphaBlend;
    public bool Enable = true;
}

public class CpFinalCamera : MonoBehaviour
{
    public List<CpDrawRenderTextureParam> RendetTextures;

    void OnGUI()
    {
        GL.Clear(false, true, Color.black);
        Rect Rect = new Rect(0, 0, Screen.width, Screen.height);
        foreach (CpDrawRenderTextureParam param in RendetTextures)
        {
            param.Draw(Rect);
        }
    }
}
