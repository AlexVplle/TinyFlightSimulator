using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public bool printTimers;

    [Header("Mesh Settings")]
    public int mapSize = 200;
    public float scale = 4000;
    public float elevationScale = 400;
    public Material material;
    public float quality = 0.8f;

    [Header("Erosion Settings")]
    public ComputeShader erosion;
    public int numErosionIterations = 5000;
    public int erosionBrushRadius = 6;
    public int maxLifetime = 90;
    public float sedimentCapacityFactor = 3;
    public float minSedimentCapacity = 0;
    public float depositSpeed = 0.3f;
    public float erodeSpeed = 0.3f;
    public float evaporateSpeed = 0.02f;
    public float gravity = 4;
    public float startSpeed = 1;
    public float startWater = 1;
    [Range(0, 1)]
    public float inertia = 0.065f;

    [Header("Vegetation Settings")]
    public GameObject[] treePrefabs;
    public int numberOfTrees = 1000;
    public float treePlacementHeightThreshold = 0.2f;
    public float minTreeScale = 2;
    public float maxTreeScale = 5;

    // Internal
    float[] map;
    Mesh mesh;
    int mapSizeWithBorder;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    List<GameObject> generatedTrees = new List<GameObject>(); // List to store generated trees

    public void Simplify()
    {
        mesh.name = "Terrain Mesh";
        var meshSimplifier = new UnityMeshSimplifier.MeshSimplifier();
        meshSimplifier.Initialize(mesh);
        meshSimplifier.SimplifyMesh(quality);
        var destMesh = meshSimplifier.ToMesh();
        mesh = destMesh;
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    public void GenerateHeightMap()
    {
        mapSizeWithBorder = mapSize + erosionBrushRadius * 2;
        map = GetComponent<HeightMapGenerator>().GenerateHeightMap(mapSizeWithBorder);
    }

    public void Erode()
    {
        int numThreads = numErosionIterations / 1024;

        // Create brush
        List<int> brushIndexOffsets = new List<int>();
        List<float> brushWeights = new List<float>();

        float weightSum = 0;
        for (int brushY = -erosionBrushRadius; brushY <= erosionBrushRadius; brushY++)
        {
            for (int brushX = -erosionBrushRadius; brushX <= erosionBrushRadius; brushX++)
            {
                float sqrDst = brushX * brushX + brushY * brushY;
                if (sqrDst < erosionBrushRadius * erosionBrushRadius)
                {
                    brushIndexOffsets.Add(brushY * mapSize + brushX);
                    float brushWeight = 1 - Mathf.Sqrt(sqrDst) / erosionBrushRadius;
                    weightSum += brushWeight;
                    brushWeights.Add(brushWeight);
                }
            }
        }
        for (int i = 0; i < brushWeights.Count; i++)
        {
            brushWeights[i] /= weightSum;
        }

        // Send brush data to compute shader
        ComputeBuffer brushIndexBuffer = new ComputeBuffer(brushIndexOffsets.Count, sizeof(int));
        ComputeBuffer brushWeightBuffer = new ComputeBuffer(brushWeights.Count, sizeof(int));
        brushIndexBuffer.SetData(brushIndexOffsets);
        brushWeightBuffer.SetData(brushWeights);
        erosion.SetBuffer(0, "brushIndices", brushIndexBuffer);
        erosion.SetBuffer(0, "brushWeights", brushWeightBuffer);

        // Generate random indices for droplet placement
        int[] randomIndices = new int[numErosionIterations];
        for (int i = 0; i < numErosionIterations; i++)
        {
            int randomX = Random.Range(erosionBrushRadius, mapSize + erosionBrushRadius);
            int randomY = Random.Range(erosionBrushRadius, mapSize + erosionBrushRadius);
            randomIndices[i] = randomY * mapSize + randomX;
        }

        // Send random indices to compute shader
        ComputeBuffer randomIndexBuffer = new ComputeBuffer(randomIndices.Length, sizeof(int));
        randomIndexBuffer.SetData(randomIndices);
        erosion.SetBuffer(0, "randomIndices", randomIndexBuffer);

        // Heightmap buffer
        ComputeBuffer mapBuffer = new ComputeBuffer(map.Length, sizeof(float));
        mapBuffer.SetData(map);
        erosion.SetBuffer(0, "map", mapBuffer);

        // Settings
        erosion.SetInt("borderSize", erosionBrushRadius);
        erosion.SetInt("mapSize", mapSizeWithBorder);
        erosion.SetInt("brushLength", brushIndexOffsets.Count);
        erosion.SetInt("maxLifetime", maxLifetime);
        erosion.SetFloat("inertia", inertia);
        erosion.SetFloat("sedimentCapacityFactor", sedimentCapacityFactor);
        erosion.SetFloat("minSedimentCapacity", minSedimentCapacity);
        erosion.SetFloat("depositSpeed", depositSpeed);
        erosion.SetFloat("erodeSpeed", erodeSpeed);
        erosion.SetFloat("evaporateSpeed", evaporateSpeed);
        erosion.SetFloat("gravity", gravity);
        erosion.SetFloat("startSpeed", startSpeed);
        erosion.SetFloat("startWater", startWater);

        // Run compute shader
        erosion.Dispatch(0, numThreads, 1, 1);
        mapBuffer.GetData(map);

        // Release buffers
        mapBuffer.Release();
        randomIndexBuffer.Release();
        brushIndexBuffer.Release();
        brushWeightBuffer.Release();
    }

    public void ContructMesh()
    {
        DestroyGeneratedTrees(); // Destroy previously generated trees

        Vector3[] verts = new Vector3[mapSize * mapSize];
        int[] triangles = new int[(mapSize - 1) * (mapSize - 1) * 6];
        int t = 0;

        for (int i = 0; i < mapSize * mapSize; i++)
        {
            int x = i % mapSize;
            int y = i / mapSize;
            int borderedMapIndex = (y + erosionBrushRadius) * mapSizeWithBorder + x + erosionBrushRadius;
            int meshMapIndex = y * mapSize + x;

            Vector2 percent = new Vector2(x / (mapSize - 1f), y / (mapSize - 1f));
            Vector3 pos = new Vector3(percent.x * 2 - 1, 0, percent.y * 2 - 1) * scale;

            float normalizedHeight = map[borderedMapIndex];
            pos += Vector3.up * normalizedHeight * elevationScale;
            verts[meshMapIndex] = pos;

            // Construct triangles
            if (x != mapSize - 1 && y != mapSize - 1)
            {
                t = (y * (mapSize - 1) + x) * 3 * 2;

                triangles[t + 0] = meshMapIndex + mapSize;
                triangles[t + 1] = meshMapIndex + mapSize + 1;
                triangles[t + 2] = meshMapIndex;

                triangles[t + 3] = meshMapIndex + mapSize + 1;
                triangles[t + 4] = meshMapIndex + 1;
                triangles[t + 5] = meshMapIndex;
                t += 6;
            }
        }

        if (mesh == null)
        {
            mesh = new Mesh();
        }
        else
        {
            mesh.Clear();
        }
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = verts;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        AssignMeshComponents();
        meshFilter.sharedMesh = mesh;
        meshRenderer.sharedMaterial = material;

        material.SetFloat("_MaxHeight", elevationScale);

        // Update the mesh collider
        meshCollider.sharedMesh = mesh;

        // Add trees to the terrain
        AddTrees();
    }

    void AssignMeshComponents()
    {
        // Find/create mesh holder object in children
        string meshHolderName = "Mesh Holder";
        Transform meshHolder = transform.Find(meshHolderName);
        if (meshHolder == null)
        {
            meshHolder = new GameObject(meshHolderName).transform;
            meshHolder.transform.parent = transform;
            meshHolder.transform.localPosition = Vector3.zero;
            meshHolder.transform.localRotation = Quaternion.identity;
        }

        // Ensure mesh renderer and filter components are assigned
        if (!meshHolder.gameObject.GetComponent<MeshFilter>())
        {
            meshHolder.gameObject.AddComponent<MeshFilter>();
        }
        if (!meshHolder.GetComponent<MeshRenderer>())
        {
            meshHolder.gameObject.AddComponent<MeshRenderer>();
        }

        // Ensure mesh collider component is assigned
        if (!meshHolder.gameObject.GetComponent<MeshCollider>())
        {
            meshHolder.gameObject.AddComponent<MeshCollider>();
        }

        meshRenderer = meshHolder.GetComponent<MeshRenderer>();
        meshFilter = meshHolder.GetComponent<MeshFilter>();
        meshCollider = meshHolder.GetComponent<MeshCollider>();
    }

    void AddTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            float randomX = Random.Range(0, mapSize);
            float randomZ = Random.Range(0, mapSize);
            float height = map[(int)randomZ * mapSize + (int)randomX];
            if (height > treePlacementHeightThreshold)
            {
                // Convert heightmap coordinates to mesh coordinates
                float percentX = randomX / (mapSize - 1f);
                float percentZ = randomZ / (mapSize - 1f);

                float posX = percentX * scale - scale / 2;
                float posZ = percentZ * scale - scale / 2;
                float posY = height * elevationScale;

                Vector3 position = new Vector3(posX, posY + 1, posZ); // Adding 1 to make sure it is above the mesh to raycast down

                // Perform a raycast down to find the exact height of the terrain
                RaycastHit hit;
                if (Physics.Raycast(position, Vector3.down, out hit))
                {
                    position.y = hit.point.y;

                    GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                    GameObject treeInstance = Instantiate(treePrefab, position, Quaternion.identity);

                    // Adjust the scale of the tree if needed
                    float scaleMultiplier = Random.Range(minTreeScale, maxTreeScale);
                    treeInstance.transform.localScale *= scaleMultiplier;

                    // Ensure the tree is properly placed on the ground
                    treeInstance.transform.position = position;
                    treeInstance.transform.parent = transform;

                    // Add the tree to the list of generated trees
                    generatedTrees.Add(treeInstance);
                }
            }
        }
    }

    void DestroyGeneratedTrees()
    {
        foreach (GameObject tree in generatedTrees)
        {
            if (Application.isPlaying)
            {
                Destroy(tree);
            }
            else
            {
                DestroyImmediate(tree);
            }
        }
        generatedTrees.Clear();
    }
}
