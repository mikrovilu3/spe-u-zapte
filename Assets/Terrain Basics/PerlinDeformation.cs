using UnityEngine;
using System.Collections.Generic;

public class MeshSubdivideAndDeform : MonoBehaviour
{
    [Header("Color Settings")]
    public Gradient heightGradient;

    [Header("Subdivision Settings")]
    public int subdivisions = 2;

    [Header("Perlin Noise Settings")]
    public float noiseScale = 0.5f;
    public float noiseSpeed = 1f;
    public float noiseStrength = 0.01f;
    public Vector2 noiseOffset = Vector2.zero;
    public bool flipValues = false;
    public Vector2 waveDirection = new Vector2(1f, 1f);

    private Mesh mesh;
    private Vector3[] originalVertices;
    private MeshCollider meshCollider;

    void Start()
    {
        mesh = Instantiate(GetComponent<MeshFilter>().mesh);
        mesh = SubdivideMesh(mesh);
        originalVertices = mesh.vertices;
        GetComponent<MeshFilter>().mesh = mesh;

        meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    void Update()
    {
        ApplyPerlinNoise();
        ApplyVertexColorsFromHeight();
    }

    Mesh SubdivideMesh(Mesh originalMesh)
    {
        Mesh mesh = Instantiate(originalMesh);

        for (int s = 0; s < subdivisions; s++)
        {
            List<Vector3> oldVerts = new List<Vector3>(mesh.vertices);
            List<int> oldTris = new List<int>(mesh.triangles);
            Dictionary<long, int> midpointCache = new Dictionary<long, int>();
            List<Vector3> newVerts = new List<Vector3>(oldVerts);
            List<int> newTris = new List<int>();

            for (int i = 0; i < oldTris.Count; i += 3)
            {
                int i0 = oldTris[i];
                int i1 = oldTris[i + 1];
                int i2 = oldTris[i + 2];

                int a = GetMidpointIndex(midpointCache, newVerts, i0, i1);
                int b = GetMidpointIndex(midpointCache, newVerts, i1, i2);
                int c = GetMidpointIndex(midpointCache, newVerts, i2, i0);

                newTris.Add(i0); newTris.Add(a); newTris.Add(c);
                newTris.Add(i1); newTris.Add(b); newTris.Add(a);
                newTris.Add(i2); newTris.Add(c); newTris.Add(b);
                newTris.Add(a); newTris.Add(b); newTris.Add(c);
            }

            mesh.vertices = newVerts.ToArray();
            mesh.triangles = newTris.ToArray();
            mesh.RecalculateNormals();
        }

        return mesh;
    }

    // Helper for midpoint caching
    int GetMidpointIndex(Dictionary<long, int> cache, List<Vector3> verts, int i0, int i1)
    {
        long key = ((long)Mathf.Min(i0, i1) << 32) + Mathf.Max(i0, i1);
        if (cache.TryGetValue(key, out int idx))
            return idx;

        Vector3 v0 = verts[i0];
        Vector3 v1 = verts[i1];
        Vector3 mid = (v0 + v1) * 0.5f;
        verts.Add(mid);
        int newIndex = verts.Count - 1;
        cache[key] = newIndex;
        return newIndex;
    }

    void ApplyPerlinNoise()
    {
        Vector3[] vertices = new Vector3[originalVertices.Length];
        float timeFactor = Time.time * noiseSpeed;

    for (int i = 0; i < vertices.Length; i++)
    {
        Vector3 vertex = originalVertices[i];

        float noise = Mathf.PerlinNoise(
            vertex.x * noiseScale + timeFactor * waveDirection.x + noiseOffset.x,
            vertex.z * noiseScale + timeFactor * waveDirection.y + noiseOffset.y
        );

        float centeredNoise = (noise - 0.5f) * noiseStrength;

        if (flipValues) centeredNoise *= -1; // ✅ Flip deformation if toggle is enabled

        vertex.y += centeredNoise;

        vertices[i] = vertex;
    }

    mesh.vertices = vertices;
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
    }

void ApplyVertexColorsFromHeight()
{
    Vector3[] vertices = mesh.vertices;
    Color[] colors = new Color[vertices.Length];

    float minHeight = float.MaxValue;
    float maxHeight = float.MinValue;

    // First, get the min and max height
    for (int i = 0; i < vertices.Length; i++)
    {
        float y = vertices[i].y;
        if (y < minHeight) minHeight = y;
        if (y > maxHeight) maxHeight = y;
    }

    // Now apply gradient color based on normalized height
    for (int i = 0; i < vertices.Length; i++)
    {
        float normalizedHeight = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
        colors[i] = heightGradient.Evaluate(normalizedHeight);
    }

    mesh.colors = colors;
}


}
