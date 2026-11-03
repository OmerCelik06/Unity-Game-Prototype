using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class AutoWorldUV : MonoBehaviour
{
    public float scale = 0.5f;

    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();

        if (mf == null)
        {
            Debug.LogError($"{gameObject.name} MeshFilter bulunamadý);
            return;
        }

        Mesh originalMesh = mf.sharedMesh;

        if (originalMesh == null)
        {
            Debug.LogError($"{gameObject.name} objenin MeshFilter'ý boþ);
            return;
        }

        Mesh newMesh = Instantiate(originalMesh);

        Vector3[] vertices = newMesh.vertices;
        Vector3[] normals = newMesh.normals;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(vertices[i]);
            Vector3 normal = transform.TransformDirection(normals[i]).normalized;

            if (Mathf.Abs(normal.z) > 0.5f)
                uvs[i] = new Vector2(worldPos.x, worldPos.y) * scale;
            else if (Mathf.Abs(normal.x) > 0.5f)
                uvs[i] = new Vector2(worldPos.z, worldPos.y) * scale;
            else
                uvs[i] = new Vector2(worldPos.x, worldPos.z) * scale;
        }

        newMesh.uv = uvs;
        mf.mesh = newMesh;
    }
}