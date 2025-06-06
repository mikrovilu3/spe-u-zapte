using UnityEngine;
using System.Collections.Generic;

public class MeshSubdivideAndDeform : MonoBehaviour
{
    [Header("Color Settings")]
    public Gradient heightGradient;

    [Header("Subdivision Settings")]
    public int subdivisions = 2;

    [Header("3D Perlin Noise Settings")]
    public float noiseScale = 0.5f;
    public float noiseSpeed = 1f;
    public float noiseStrength = 0.01f;
    public Vector3 noiseOffset = Vector3.zero;
    public bool flipValues = false;
    public Vector3 waveDirection = new Vector3(1f, 1f, 0.5f);
    public Vector3 waveDirection2 = new Vector3(-0.5f, 1f, 1f);

    [Header("3D Noise Controls")]
    public float timeScale = 0.1f; // Controls how fast the 3D noise evolves
    public bool useTimeAsThirdDimension = true; // Use time for Z dimension in noise
    public float staticZOffset = 0f; // Static Z offset if not using time

    [Header("Chaos Noise Settings")]
    public bool enableChaosNoise = true;
    public float chaosStrength = 0.005f;
    public float chaosFrequency = 10f; // Higher = more rapid changes
    public Vector3 chaosScale = new Vector3(1f, 1f, 1f); // Per-axis chaos scaling
    public bool useVertexIndexForChaos = true; // Use vertex index for consistent chaos per vertex

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
    private float chaosStrengthMultiplier;

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
        chaosStrengthMultiplier = flipValues ? -chaosStrength : chaosStrength;
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
            chaosStrengthMultiplier = flipValues ? -chaosStrength : chaosStrength;

            Apply3DPerlinNoise();

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

    // 3D Perlin noise implementation using Unity's built-in noise with layering
    float PerlinNoise3D(float x, float y, float z)
    {
        // Unity doesn't have built-in 3D Perlin noise, so we layer 2D noise
        // This creates a pseudo-3D effect by combining multiple 2D samples
        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);
        float yz = Mathf.PerlinNoise(y, z);

        // Average the three 2D noise values to simulate 3D
        return (xy + xz + yz) / 3f;
    }

    // Generate chaos noise for additional randomness
    Vector3 GenerateChaosNoise(int vertexIndex, Vector3 worldPosition)
    {
        if (!enableChaosNoise) return Vector3.zero;

        // Create seeds based on vertex index and time for consistent but evolving chaos
        float seed = useVertexIndexForChaos ? vertexIndex * 0.1f : 0f;
        float timeOffset = timeFactor * chaosFrequency;

        // Generate random values using different seeds for each axis
        float chaosX = (Mathf.Sin((worldPosition.x + seed + timeOffset) * chaosFrequency) +
                       Mathf.Cos((worldPosition.y + seed * 2f + timeOffset) * chaosFrequency * 1.3f)) * 0.5f;

        float chaosY = (Mathf.Sin((worldPosition.y + seed * 3f + timeOffset) * chaosFrequency * 1.1f) +
                       Mathf.Cos((worldPosition.z + seed + timeOffset) * chaosFrequency * 0.9f)) * 0.5f;

        float chaosZ = (Mathf.Sin((worldPosition.z + seed * 2f + timeOffset) * chaosFrequency * 0.8f) +
                       Mathf.Cos((worldPosition.x + seed * 4f + timeOffset) * chaosFrequency * 1.2f)) * 0.5f;

        return new Vector3(
            chaosX * chaosScale.x * chaosStrengthMultiplier,
            chaosY * chaosScale.y * chaosStrengthMultiplier,
            chaosZ * chaosScale.z * chaosStrengthMultiplier
        );
    }

    void Apply3DPerlinNoise()
    {
        int vertexCount = originalVertices.Length;

        // Calculate 3D wave directions with time factor
        Vector3 waveDir1 = timeFactor * waveDirection + noiseOffset;
        Vector3 waveDir2 = timeFactor * waveDirection2 + noiseOffset;

        // Third dimension for noise (time-based or static)
        float zDimension1 = useTimeAsThirdDimension ? timeFactor * timeScale : staticZOffset;
        float zDimension2 = useTimeAsThirdDimension ? timeFactor * timeScale * 1.3f : staticZOffset + 100f; // Offset second layer

        for (int i = 0; i < vertexCount; i++)
        {
            Vector3 vertex = originalVertices[i];

            // Scale vertex positions
            float vertexXScaled = vertex.x * noiseScale;
            float vertexYScaled = vertex.y * noiseScale;
            float vertexZScaled = vertex.z * noiseScale;

            // Apply 3D noise to Y-axis
            float noiseY = PerlinNoise3D(
                vertexXScaled + waveDir1.x,
                vertexZScaled + waveDir1.y,
                zDimension1 + waveDir1.z
            );
            float noiseY2 = PerlinNoise3D(
                vertexXScaled + waveDir2.x,
                vertexZScaled + waveDir2.y,
                zDimension2 + waveDir2.z
            );
            float centeredNoiseY = ((noiseY + noiseY2) / 2f - 0.5f) * noiseStrengthMultiplier;
            vertex.y += centeredNoiseY;

            // Apply 3D noise to X-axis
            float noiseX = PerlinNoise3D(
                vertexYScaled + waveDir1.y,
                vertexZScaled + waveDir1.z,
                zDimension1 + waveDir1.x
            );
            float noiseX2 = PerlinNoise3D(
                vertexYScaled + waveDir2.y,
                vertexZScaled + waveDir2.z,
                zDimension2 + waveDir2.x
            );
            float centeredNoiseX = ((noiseX + noiseX2) / 2f - 0.5f) * noiseStrengthMultiplier;
            vertex.x += centeredNoiseX;

            // Apply 3D noise to Z-axis
            float noiseZ = PerlinNoise3D(
                vertexXScaled + waveDir1.z,
                vertexYScaled + waveDir1.x,
                zDimension1 + waveDir1.y
            );
            float noiseZ2 = PerlinNoise3D(
                vertexXScaled + waveDir2.z,
                vertexYScaled + waveDir2.x,
                zDimension2 + waveDir2.y
            );
            float centeredNoiseZ = ((noiseZ + noiseZ2) / 2f - 0.5f) * noiseStrengthMultiplier;
            vertex.z += centeredNoiseZ;

            // Add chaos noise for additional randomness
            Vector3 chaosOffset = GenerateChaosNoise(i, vertex);
            vertex += chaosOffset;

            workingVertices[i] = vertex;
        }

        mesh.vertices = workingVertices;
        mesh.RecalculateNormals();

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