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

    private const float SLOW_SPEED = 2.5f;
    private const float NORMAL_SPEED = 5f;
    private const float HIGH_SPEED = 10f;
    private float moveSpeed;

    public const float LANE_LENGTH = 5f;
    public const int NUMBER_OF_LANES = 3;
    private const int TILES_PER_LANE = 10;

    private Vector3 leftLanePosition;
    private Vector3 middleLanePosition;
    private Vector3 rightLanePosition;

    private GameObject lastLeftLaneTile;
    private GameObject lastMiddleLaneTile;
    private GameObject lastRightLaneTile;

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

        leftLanePosition = Vector3.left * Shared.LANE_WIDTH;
        middleLanePosition = Vector3.zero;
        rightLanePosition = Vector3.right * Shared.LANE_WIDTH;

        SpawnInitialTiles();
    }

    void Update()
    {
        MoveTilesTowardsPlayer();
    }

    private void SpawnInitialTiles()
    {
        for (int i = 0; i < TILES_PER_LANE; i++)
        {
            for (int j = 0; j < NUMBER_OF_LANES; j++)
            {
                SpawnTile(j, normalTilePrefab);
            }
        }
    }

    public void SpawnTile(int laneIndex, GameObject tilePrefab)
    {
        Vector3 spawnPosition = Vector3.zero;

        switch (laneIndex)
        {
            case 0:
                if (lastLeftLaneTile == null)
                {
                    spawnPosition = leftLanePosition + Vector3.back * LANE_LENGTH;
                } else
                {
                    spawnPosition = leftLanePosition + Vector3.forward * (lastLeftLaneTile.transform.position.z + LANE_LENGTH); // Spawn after last tile
                }
                break;
            case 1:
                if (lastMiddleLaneTile == null)
                {
                    spawnPosition = middleLanePosition + Vector3.back * LANE_LENGTH;
                }
                else
                {
                    spawnPosition = middleLanePosition + Vector3.forward * (lastMiddleLaneTile.transform.position.z + LANE_LENGTH); // Spawn after last tile
                }
                break;
            case 2:
                if (lastRightLaneTile == null)
                {
                    spawnPosition = rightLanePosition + Vector3.back * LANE_LENGTH;
                }
                else
                {
                    spawnPosition = rightLanePosition + Vector3.forward * (lastRightLaneTile.transform.position.z + LANE_LENGTH); // Spawn after last tile
                }
                break;
        }

        GameObject newTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);

        TileScript tileScriptComponent = newTile.GetComponent<TileScript>();
        tileScriptComponent.laneIndex = laneIndex;

        switch (laneIndex)
        {
            case 0:
                lastLeftLaneTile = newTile;
                break;
            case 1:
                lastMiddleLaneTile = newTile;
                break;
            case 2:
                lastRightLaneTile = newTile;
                break;
        }

        tiles.Add(newTile);
    }

    private GameObject GetRandomTilePrefab()
    {
        return tilePrefabs[Random.Range(0, tilePrefabs.Length)];
    }

    private void MoveTilesTowardsPlayer()
    {
        foreach (GameObject tile in tiles)
        {
            tile.transform.position += moveSpeed * Time.deltaTime * Vector3.back;
        }
    }

    public void DestroyAndReplaceTile(GameObject tile)
    {
        tiles.Remove(tile);
        
        if (tile.TryGetComponent<TileScript>(out var tileScriptComponent))
        {
            // Spawn a new tile in the same lane
            GameObject newTilePrefab = GetRandomTilePrefab();
            int laneIndex = tileScriptComponent.laneIndex;
            SpawnTile(laneIndex, newTilePrefab);
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
