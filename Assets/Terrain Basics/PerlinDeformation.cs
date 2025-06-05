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
    public Vector2 waveDirection2 = new Vector2(1f, 1f);

    [Header("Performance Settings")]
    [Range(1, 10)]
    public int updateFrequency = 2; // Update every N frames

    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] workingVertices; // Reusable array
    private Color[] workingColors; // Reusable array
    private MeshCollider meshCollider;

    // Performance optimization variables
    private int frameCounter = 0;
    private float minHeight, maxHeight;
    private bool hasHeightGradient;

    // Cached calculations
    private float timeFactor;
    private float noiseStrengthMultiplier;

    void Start()
    {
        mesh = Instantiate(GetComponent<MeshFilter>().mesh);
        mesh = SubdivideMesh(mesh);
        originalVertices = mesh.vertices;

        // Pre-allocate arrays to avoid garbage collection
        workingVertices = new Vector3[originalVertices.Length];
        workingColors = new Color[originalVertices.Length];

        GetComponent<MeshFilter>().mesh = mesh;

        meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
            meshCollider.sharedMesh = mesh;

        hasHeightGradient = heightGradient != null;
        noiseStrengthMultiplier = flipValues ? -noiseStrength : noiseStrength;
    }

    void Update()
    {
        // Only update every N frames for better performance
        frameCounter++;
        if (frameCounter >= updateFrequency)
        {
            frameCounter = 0;

            // Cache time calculation
            timeFactor = Time.time * noiseSpeed;

            // Update noise strength multiplier if flip toggle changed
            noiseStrengthMultiplier = flipValues ? -noiseStrength : noiseStrength;

            ApplyPerlinNoise();

            if (hasHeightGradient)
                ApplyVertexColorsFromHeight();
        }
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
        // Use pre-allocated array instead of creating new one
        int vertexCount = originalVertices.Length;

        // Cache wave direction calculations
        float waveDir1X = timeFactor * waveDirection.x + noiseOffset.x;
        float waveDir1Y = timeFactor * waveDirection.y + noiseOffset.y;
        float waveDir2X = timeFactor * waveDirection2.x + noiseOffset.x;
        float waveDir2Y = timeFactor * waveDirection2.y + noiseOffset.y;

        for (int i = 0; i < vertexCount; i++)
        {
            Vector3 vertex = originalVertices[i];

            // Cache vertex position calculations
            float vertexXScaled = vertex.x * noiseScale;
            float vertexZScaled = vertex.z * noiseScale;

            float noise = Mathf.PerlinNoise(
                vertexXScaled + waveDir1X,
                vertexZScaled + waveDir1Y
            );

            float noise2 = Mathf.PerlinNoise(
                vertexXScaled + waveDir2X,
                vertexZScaled + waveDir2Y
            );

            // Optimized noise calculation
            float centeredNoise = (noise + noise2 - 0.5f) * noiseStrengthMultiplier;
            vertex.y += centeredNoise;

            workingVertices[i] = vertex;
        }

        mesh.vertices = workingVertices;
        mesh.RecalculateNormals();

        // Only update collider if it exists
        if (meshCollider != null)
            meshCollider.sharedMesh = mesh;
    }

    void ApplyVertexColorsFromHeight()
    {
        int vertexCount = workingVertices.Length;

        // Find min/max height in single pass
        minHeight = float.MaxValue;
        maxHeight = float.MinValue;

        for (int i = 0; i < vertexCount; i++)
        {
            float y = workingVertices[i].y;
            if (y < minHeight) minHeight = y;
            if (y > maxHeight) maxHeight = y;
        }

        // Apply colors in second pass
        float heightRange = maxHeight - minHeight;
        if (heightRange > 0f) // Avoid division by zero
        {
            float invRange = 1f / heightRange;
            for (int i = 0; i < vertexCount; i++)
            {
                float normalizedHeight = (workingVertices[i].y - minHeight) * invRange;
                workingColors[i] = heightGradient.Evaluate(normalizedHeight);
            }
        }
        else
        {
            // All vertices at same height
            Color singleColor = heightGradient.Evaluate(0.5f);
            for (int i = 0; i < vertexCount; i++)
            {
                workingColors[i] = singleColor;
            }
        }

        mesh.colors = workingColors;
    }
}