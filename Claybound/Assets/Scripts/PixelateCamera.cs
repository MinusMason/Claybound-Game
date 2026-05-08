using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class PixelateCamera : MonoBehaviour
{
    [Range(60, 480)]
    public int targetHeight = 180;

    private RenderTexture rt;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int targetWidth = Mathf.RoundToInt(targetHeight * ((float)Screen.width / Screen.height));

        // Recreate the RenderTexture if the resolution changed
        if (rt == null || rt.height != targetHeight || rt.width != targetWidth)
        {
            if (rt != null) rt.Release();
            rt = new RenderTexture(targetWidth, targetHeight, 0)
            {
                filterMode = FilterMode.Point, // Point for harder pixing and no blurring
                antiAliasing = 1
            };
        }

        Graphics.Blit(source, rt); // Downscale to a lower resolution
        Graphics.Blit(rt, destination);  // Upscale back up to a screen with harder pixels
    }

    private void OnDisable()
    {
        if (rt != null)
        {
            rt.Release();
            rt = null;
        }
    }
}
