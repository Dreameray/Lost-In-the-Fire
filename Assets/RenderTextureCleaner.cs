using UnityEngine;

public class RenderTextureCleaner : MonoBehaviour
{
    public RenderTexture targetRenderTexture;

    void Start()
    {
        if (targetRenderTexture != null)
        {
            RenderTexture.active = targetRenderTexture;
            GL.Clear(true, true, Color.black); // Clear with black
            RenderTexture.active = null;
        }
    }
}
