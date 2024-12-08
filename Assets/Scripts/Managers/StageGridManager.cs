using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class StageGridManager : Singleton<StageGridManager>
{
    [SerializeField]
    private Tilemap _groundTilemap;
    [SerializeField]
    private Tilemap _wallTilemap;

    //Tilemap that contains the difference between the groundtilemap and the walltilemap
    private List<Vector2> _freeTiles;

    [Space(20)]
    [Header("Obstacles Spawner")]
    [SerializeField]
    private Transform _obstaclesParent;
    [SerializeField]
    private GameObject _obstaclePrefab;
    [SerializeField]
    private float _obstacleSpawnChance;
    private List<Vector2> _obstaclesTiles;

    [Space(20)]
    [Header("Debug")]
    [SerializeField]
    private bool _onDebug;
    [SerializeField]
    private GameObject _debugBlueSquare;

    private void Start()
    {
        CreateFilteredTilemap();
    }

    private void CreateFilteredTilemap()
    {
        _freeTiles = new List<Vector2>();

        AddTilesPositions(_groundTilemap, _freeTiles);
        SubtractTilesPositions(_wallTilemap, _freeTiles);
        SpawnObstacles();

        Invoke(nameof(UpdateAStar), 0.5f);
    }

    private void UpdateAStar() 
    {
        AstarPath.active.Scan();
    }

    private void AddTilesPositions(Tilemap tilemap, List<Vector2> filteredTiles)
    {
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector2 posVec = new Vector2(x, y);

                Vector3Int cellPosition = tilemap.WorldToCell(posVec);

                if (tilemap.HasTile(cellPosition))
                {
                    Vector3 cellWorldPosition = tilemap.GetCellCenterWorld(cellPosition);
                    filteredTiles.Add(cellWorldPosition);
                }
            }
        }
    }

    private void SubtractTilesPositions(Tilemap tilemap, List<Vector2> filteredTiles)
    {
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector2 posVec = new Vector2(x, y);

                Vector3Int cellPosition = tilemap.WorldToCell(posVec);
                Vector3 cellWorldPosition = tilemap.GetCellCenterWorld(cellPosition);

                if (tilemap.HasTile(cellPosition) && filteredTiles.Contains(cellWorldPosition))
                {
                    filteredTiles.Remove(cellWorldPosition);
                }
            }
        }
    }

    private void SpawnObstacles()
    {
        int count = _obstaclesParent.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(_obstaclesParent.GetChild(i).gameObject);
        }

        _obstaclesTiles = new List<Vector2>();

        foreach (Vector2 freeTile in _freeTiles.ToArray())
        {
            float chance = Random.Range(0f, 100f);
            if (chance <= _obstacleSpawnChance)
            {
                Vector2 freeTilePos = freeTile;

                Vector3Int cellPosition = _groundTilemap.WorldToCell(freeTilePos);
                Vector3 spawnPos = _groundTilemap.GetCellCenterWorld(cellPosition);

                Instantiate(_obstaclePrefab, spawnPos, Quaternion.identity, _obstaclesParent);
                if(_onDebug)
                    Instantiate(_debugBlueSquare, spawnPos, Quaternion.identity, _obstaclesParent);
                _freeTiles.Remove(freeTile);
                _obstaclesTiles.Add(freeTile);
            }
        }
    }

    public Vector2 FindClosestFreeTile(Vector2 pos)
    {
        Vector2 closestFreeTile = new Vector2(int.MaxValue, int.MaxValue);

        foreach (Vector2 tile in _freeTiles)
        {
            if (Vector2.Distance(pos, tile) < Vector2.Distance(pos, closestFreeTile))
            {
                closestFreeTile = tile;
            }
        }

        return closestFreeTile;
    }

    public Vector2 FindClosestFreeTileAdjacentToPosition(Vector2 targetPosition, Vector2 characterPosition)
    {
        Vector2[] adjacentPositions = new Vector2[]
        {
            targetPosition + Vector2.up,
            targetPosition + Vector2.down,
            targetPosition + Vector2.left,
            targetPosition + Vector2.right
        };

        Vector2 closestFreeTile = characterPosition;
        float shortestDistance = float.MaxValue;

        foreach (Vector2 pos in adjacentPositions)
        {
            if (!_freeTiles.Contains(pos))
                continue;

            float distance = Vector2.Distance(pos, characterPosition);

            if (distance < shortestDistance) 
            {
                shortestDistance = distance;
                closestFreeTile = pos;
            }
        }

        return closestFreeTile;
    }

    public bool HasObstacles()
    {
        return _obstaclesTiles.Count > 0;
    }

    public Vector2 FindClosestObstacle(Vector2 pos)
    {
        Vector2 closestObstacle = new Vector2(int.MaxValue, int.MaxValue);

        foreach (Vector2 tile in _obstaclesTiles)
        {
            if (Vector2.Distance(pos, tile) < Vector2.Distance(pos, closestObstacle))
            {
                closestObstacle = tile;
            }
        }

        return closestObstacle;
    }

    public void RemoveObstacleFromTilemap(Vector2 obstaclePos)
    {
        Vector3Int cellPosition = _groundTilemap.WorldToCell(obstaclePos);
        _obstaclesTiles.Remove((Vector2Int)cellPosition);
    }

    public Tilemap GetGroundTilemap()
    {
        return _groundTilemap;
    }

    public List<Vector2> GetFreeTiles() 
    {
        return _freeTiles;
    }
}
