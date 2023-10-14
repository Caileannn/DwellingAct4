using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class MeshVFXBinder : MonoBehaviour
{
    private VisualEffect vfx;
    private MeshFilter meshFilter;
    private Renderer render;

    private static class ID
    {
        public static int Mesh = Shader.PropertyToID("Mesh");
    }

    void Update()
    {
        if (vfx == null) vfx = GetComponent<VisualEffect>();
        if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
        if (meshFilter.sharedMesh.isReadable == false) Debug.LogError("Mesh needs to be Read/Write enabled in import options");

        if (vfx.HasMesh(ID.Mesh)) vfx.SetMesh(ID.Mesh, meshFilter.sharedMesh);
    }
}