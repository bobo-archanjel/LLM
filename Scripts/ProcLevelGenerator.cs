using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralTerrainGenerator : MonoBehaviour
{
    [Header("Tilemap References")]
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;
    public Tilemap trapTilemap;
    public Tilemap backgroundTilemap;

    [Header("Tiles")]
    public TileBase groundTile;
    public TileBase wallTile;
    public TileBase trapTile;
    public TileBase backgroundTile;

    [Header("Generation Settings")]
    public int mapWidth = 30; // Width of one level segment
    public int minHeight = 4; // Minimum terrain height
    public int maxHeight = 10; // Maximum terrain height
    public float largeScaleSmoothness = 0.05f; // Large-scale Perlin Noise (bigger hills)
    public float smallScaleSmoothness = 0.2f; // Small-scale Perlin Noise (local bumps)

    [Header("Feature Settings")]
    public GameObject enemyPrefab;
    public GameObject npcPrefab;
    public GameObject heartPrefab; // Heart prefab
    public int maxEnemiesPerZone = 3; // Max enemies in an enemy zone
    public int maxHeartsPerSegment = 2; // Max hearts per segment
    public float heartSpawnChance = 0.2f; // 20% chance to spawn a heart in a valid position

    private int currentEndX = 0;

    void Start()
    {
        Vector3 cameraBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        currentEndX = Mathf.FloorToInt(cameraBottomLeft.x); // Align map start to camera position
        GenerateSegment(currentEndX);
    }

    void Update()
    {
        // Generate new segments when the player moves closer to the end of the current map
        if (Camera.main.transform.position.x > currentEndX - (mapWidth / 2))
        {
            GenerateSegment(currentEndX);
        }
    }

    void GenerateSegment(int startX)
    {
        int startY = Mathf.FloorToInt(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y); // Bottom of the camera view
        int maxGroundHeight = 0;

        for (int x = 0; x < mapWidth; x++)
        {
            // Generate terrain height using combined Perlin Noise
            float largeScaleNoise = Mathf.PerlinNoise((x + startX) * largeScaleSmoothness, 0f);
            float smallScaleNoise = Mathf.PerlinNoise((x + startX) * smallScaleSmoothness, 0f) * 0.3f;
            int groundHeight = Mathf.Clamp(
                Mathf.RoundToInt((largeScaleNoise + smallScaleNoise) * (maxHeight - minHeight) + minHeight),
                minHeight, maxHeight
            );

            // Generate ground tiles
            for (int y = startY; y < groundHeight; y++)
            {
                groundTilemap.SetTile(new Vector3Int(x + startX, y, 0), groundTile);
            }

            // Track the highest ground tile
            if (groundHeight > maxGroundHeight)
                maxGroundHeight = groundHeight;

            // Add traps with reduced frequency
            if (Random.value < 0.05f) // 5% chance
            {
                trapTilemap.SetTile(new Vector3Int(x + startX, groundHeight, 0), trapTile);
            }
        }

        // Generate zones for enemies or NPCs
        GenerateZones(startX, maxGroundHeight);

        // Spawn hearts
        SpawnHearts(startX);

        // Refresh tilemaps
        groundTilemap.RefreshAllTiles();
        trapTilemap.RefreshAllTiles();

        // Move the end marker forward
        currentEndX += mapWidth;
    }

    void GenerateZones(int startX, int maxGroundHeight)
    {
        for (int zoneStartX = startX; zoneStartX < startX + mapWidth; zoneStartX += 10)
        {
            if (Random.value < 0.6f) // 60% chance for enemy zone
            {
                GenerateEnemyZone(zoneStartX, maxGroundHeight);
            }
            else // 40% chance for NPC zone
            {
                GenerateNPCZone(zoneStartX, maxGroundHeight);
            }
        }
    }

    void GenerateEnemyZone(int zoneStartX, int maxGroundHeight)
    {
        int enemyCount = Random.Range(1, maxEnemiesPerZone + 1); // Between 1 and maxEnemiesPerZone

        for (int i = 0; i < enemyCount; i++)
        {
            int x = Random.Range(zoneStartX, zoneStartX + 10);
            int y = FindHighestTileInColumn(x);

            if (y != -1)
            {
                Vector3 position = groundTilemap.CellToWorld(new Vector3Int(x, y, 0));
                Instantiate(enemyPrefab, position, Quaternion.identity);
            }
        }
    }

    void GenerateNPCZone(int zoneStartX, int maxGroundHeight)
    {
        int npcX = Random.Range(zoneStartX, zoneStartX + 10);
        int npcY = FindHighestTileInColumn(npcX);

        if (npcY != -1)
        {
            Vector3 position = groundTilemap.CellToWorld(new Vector3Int(npcX, npcY, 0));
            Instantiate(npcPrefab, position, Quaternion.identity);
        }
    }

    void SpawnHearts(int startX)
    {
        int heartsSpawned = 0;

        for (int x = startX; x < startX + mapWidth; x++)
        {
            if (heartsSpawned >= maxHeartsPerSegment)
                break;

            if (Random.value < heartSpawnChance) // Spawn chance
            {
                int y = FindHighestTileInColumn(x);

                if (y != -1)
                {
                    Vector3 position = groundTilemap.CellToWorld(new Vector3Int(x, y, 0));
                    Instantiate(heartPrefab, position, Quaternion.identity);
                    heartsSpawned++;
                }
            }
        }
    }

    int FindHighestTileInColumn(int x)
    {
        for (int y = maxHeight + 10; y >= 0; y--)
        {
            if (groundTilemap.HasTile(new Vector3Int(x, y, 0)))
            {
                return y + 1; // Just above the ground
            }
        }
        return -1;
    }
}
