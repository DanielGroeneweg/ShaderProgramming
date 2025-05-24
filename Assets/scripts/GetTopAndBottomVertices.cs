using UnityEngine;
using UnityEngine.UIElements;

public class GetTopAndBottomVertices : MonoBehaviour
{
    [SerializeField] private Material material;
    private void Start()
    {
        SetVertices();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        {
            SetVertices();
        }
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