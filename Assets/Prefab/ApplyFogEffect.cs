using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ApplyFogEffect : MonoBehaviour
{
    public Material postProcessMaterial;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (postProcessMaterial != null)
        {
            Graphics.Blit(src, dest, postProcessMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
