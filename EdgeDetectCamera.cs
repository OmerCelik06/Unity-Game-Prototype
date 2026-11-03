using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class EdgeDetectCamera : MonoBehaviour
{
    public Material edgeMaterial;

    [Header("Retro Ayarlarý")]
    [Range(1, 8)]
    public int downsample = 3; 

    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals | DepthTextureMode.Depth;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (edgeMaterial != null)
        {
            int width = source.width / downsample;
            int height = source.height / downsample;

            RenderTexture rt = RenderTexture.GetTemporary(width, height);

            rt.filterMode = FilterMode.Point;

            Graphics.Blit(source, rt, edgeMaterial);

            Graphics.Blit(rt, destination);

            RenderTexture.ReleaseTemporary(rt);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}