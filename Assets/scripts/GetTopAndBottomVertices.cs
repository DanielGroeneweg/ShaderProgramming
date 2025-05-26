using UnityEngine;
public class GetTopAndBottomVertices : MonoBehaviour
{
    [SerializeField] private Material material;
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

        foreach (Vector3 v in vertices)
        {
            float worldY = transform.TransformPoint(v).y;
            if (worldY < minY) minY = worldY;
            if (worldY > maxY) maxY = worldY;
        }

        material.SetFloat("_MeshMinY", minY);
        material.SetFloat("_MeshMaxY", maxY);
    }
}