using UnityEngine;

public class FogController : MonoBehaviour
{
    public ComputeShader fogShader;
    public Transform planeTransform;
    public RenderTexture fogTexture;
    private int kernelHandle;

    void Start()
    {
        kernelHandle = fogShader.FindKernel("CSMain");
        fogShader.SetTexture(kernelHandle, "Result", fogTexture);
        fogShader.SetInt("width", fogTexture.width);
        fogShader.SetInt("height", fogTexture.height);
    }

    void Update()
    {
        fogShader.SetVector("_PlanePosition", planeTransform.position);
        fogShader.SetVector("_CameraForward", transform.forward);
        fogShader.SetVector("_CameraRight", transform.right);
        fogShader.SetVector("_CameraUp", transform.up);
        fogShader.SetFloat("_FogDensity", 1.0f);

        fogShader.Dispatch(kernelHandle, fogTexture.width / 8, fogTexture.height / 8, 1);
    }
}
