using UnityEngine;

using UnityEditor;

using Codice.CM.Common;

public class CpAsserPostprocessor : AssetPostprocessor
{
    private void OnPostprocessTexture(Texture2D texture)
    {
        TextureImporter ti = assetImporter as TextureImporter;
        ti.mipmapEnabled = false;
        ti.spritePixelsPerUnit = 1;

        ti.wrapMode = TextureWrapMode.Clamp;
        ti.filterMode = FilterMode.Point;
        ti.compressionQuality = 0;
    }
};
