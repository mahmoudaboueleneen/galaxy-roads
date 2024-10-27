using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }

    [Header("Tile Prefabs")]
    [SerializeField] private GameObject normalTilePrefab;
    [SerializeField] private GameObject burningTilePrefab;
    [SerializeField] private GameObject suppliesTilePrefab;
    [SerializeField] private GameObject boostTilePrefab;
    [SerializeField] private GameObject stickyTilePrefab;
    [SerializeField] private GameObject emptyTilePrefab;
    private GameObject[] tilePrefabs;

    private List<GameObject> tiles;

    [Header("Obstacle Prefabs")]
    [SerializeField] private GameObject obstaclePrefab;

    private const int TILES_PER_LANE = 5;
    private const float SLOW_SPEED = 2.5f;
    private const float NORMAL_SPEED = 5f;
    private const float HIGH_SPEED = 10f;

    private float moveSpeed;
    private float[] furthestZInLane;

    private Vector3 leftLanePosition;
    private Vector3 middleLanePosition;
    private Vector3 rightLanePosition;

    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI speedText;


    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        tilePrefabs = new GameObject[] { 
            normalTilePrefab, 
            burningTilePrefab, 
            suppliesTilePrefab, 
            boostTilePrefab, 
            stickyTilePrefab, 
            emptyTilePrefab 
        };
        tiles = new List<GameObject>();

        moveSpeed = NORMAL_SPEED;
        speedText.text = "Normal Speed";

        furthestZInLane = new float[3] { -Shared.LANE_LENGTH, -Shared.LANE_LENGTH, -Shared.LANE_LENGTH };

        middleLanePosition = Vector3.zero;
        leftLanePosition = Vector3.left * Shared.LANE_WIDTH;
        rightLanePosition = Vector3.right * Shared.LANE_WIDTH;

        for (int laneIdx = 0; laneIdx < 3; laneIdx++)
            for (int j = 0; j < TILES_PER_LANE; j++)
                SpawnTile(laneIdx);
    }

    void Update()
    {
        MoveTilesTowardsPlayer();
    }

    public void SpawnTile(int laneIndex)
    {
        Vector3 spawnPosition = Vector3.zero;
        const float tileLength = Shared.LANE_LENGTH;

        switch (laneIndex)
        {
            case 0:
                spawnPosition = leftLanePosition + Vector3.forward * (furthestZInLane[0]);
                furthestZInLane[0] += tileLength;
                break;
            case 1:
                spawnPosition = middleLanePosition + Vector3.forward * (furthestZInLane[1]);
                furthestZInLane[1] += tileLength;
                break;
            case 2:
                spawnPosition = rightLanePosition + Vector3.forward * (furthestZInLane[2]);
                furthestZInLane[2] += tileLength;
                break;
        }

        GameObject newTilePrefab = GetRandomTilePrefab();
        GameObject newTile = Instantiate(newTilePrefab, spawnPosition, Quaternion.identity);

        TileScript tileScriptComponent = newTile.GetComponent<TileScript>();
        tileScriptComponent.laneIndex = laneIndex;

        tiles.Add(newTile);
    }

    private GameObject GetRandomTilePrefab()
    {
        return tilePrefabs[Random.Range(0, tilePrefabs.Length)];
    }

    private void MoveTilesTowardsPlayer()
    {
        foreach (GameObject tile in tiles)
            tile.transform.position += moveSpeed * Time.deltaTime * Vector3.back;
    }

    public void DestroyAndReplaceTile(GameObject tile)
    {
        tiles.Remove(tile);
        
        if (tile.TryGetComponent<TileScript>(out var tileScriptComponent))
        {
            // Spawn a new tile in the same lane
            int laneIndex = tileScriptComponent.laneIndex;
            SpawnTile(laneIndex);
        }

        Destroy(tile);
    }

    public void BoostTileSpeed()
    {
        moveSpeed = HIGH_SPEED;
        speedText.text = "High Speed";
    }

    public void ResetTileSpeed()
    {
        moveSpeed = NORMAL_SPEED;
        speedText.text = "Normal Speed";
    }
}
