using UnityEngine;
public class GetTopAndBottomVertices : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private bool giveX;
    [SerializeField] private bool giveY;
    [SerializeField] private bool giveZ;
    private void Update()
    {
        SetVertices();
    }
    private void SetVertices()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = mesh.vertices;

        float minY = float.MaxValue;
        float maxY = float.MinValue;

        float minX = float.MaxValue;
        float maxX = float.MinValue;

        float minZ = float.MaxValue;
        float maxZ = float.MinValue;

        foreach (Vector3 v in vertices)
        {
            Vector3 world = transform.TransformPoint(v);
            if (world.y < minY) minY = world.y;
            if (world.y > maxY) maxY = world.y;

            if (world.x < minX) minX = world.x;
            if (world.x > maxX) maxX = world.x;

            if (world.z < minZ) minZ = world.z;
            if (world.z > maxX) maxZ = world.z;
        }

        if (giveX)
        {
            material.SetFloat("_MeshMinX", minX);
            material.SetFloat("_MeshMaxX", maxX);
        }

        if (giveY)
        {
            material.SetFloat("_MeshMinY", minY);
            material.SetFloat("_MeshMaxY", maxY);
        }
        
        if (giveZ)
        {
            material.SetFloat("_MeshMinZ", minZ);
            material.SetFloat("_MeshMaxZ", maxZ);
        }
    }
}