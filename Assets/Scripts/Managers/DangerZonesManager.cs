using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DangerZonesManager : Singleton<DangerZonesManager>
{
    public delegate void DangerZoneUpdated();
    public DangerZoneUpdated OnDangerZoneUpdated;

    private List<Vector2> _dangerZones = new List<Vector2>();

    private Tilemap _groundTilemap;
    [Space(20)]
    [Header("Debug")]
    [SerializeField]
    private bool _onDebug;
    [SerializeField]
    private GameObject _debugRedSquare;
    private List<GameObject> _activeDebugRedSquares = new List<GameObject>();

    private void Start()
    {
        _groundTilemap = StageGridManager.instance.GetGroundTilemap();
    }

    public void UpdateDangerZone()
    {
        Bomb[] bombs = FindObjectsOfType<Bomb>();
        _dangerZones = new List<Vector2>();

        if (bombs.Length == 0)
        {
            OnDangerZoneUpdated?.Invoke();
            return;
        }

        Vector2[] directions = {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        foreach (Bomb bomb in bombs)
        {
            _dangerZones.Add(bomb.transform.position);

            for (int i = 0; i <= bomb.BombExplosionSize; i++)
            {
                foreach (Vector2 dir in directions)
                {
                    _dangerZones.Add((Vector2) bomb.transform.position + (dir * i));
                }
            }
        }

        if (_onDebug) 
        {
            foreach (GameObject go in _activeDebugRedSquares)
            {
                Destroy(go);
            }

            foreach (Vector2 dangerZone in _dangerZones)
            {
                _activeDebugRedSquares.Add(Instantiate(_debugRedSquare, dangerZone, Quaternion.identity));
            }
        }

        OnDangerZoneUpdated?.Invoke();
    }

    private Vector3 GetBombPositionInTile(Vector2Int bombPos, Tilemap groundTilemap) 
    {
        Vector2 bombPosVec2 = bombPos;
        Vector3Int bombCellPosition = groundTilemap.WorldToCell(bombPosVec2);
        Vector3 bombTilePosition = groundTilemap.GetCellCenterWorld(bombCellPosition);

        return bombTilePosition;
    }

    public Vector2 FindNearestSafeZone(Vector2 pos) 
    {
        Vector2 nearestSafeZone = pos;
        if (_dangerZones.Count == 0)
            return nearestSafeZone;

        List<Vector2> safeZones = StageGridManager.instance.GetFreeTiles();

        foreach (Vector2 dangerZone in _dangerZones)
        {
            safeZones.Remove(dangerZone);
        }

        float shortestDistance = float.MaxValue;

        foreach (Vector2 safeZone in safeZones)
        {
            float distance = Vector2.Distance(pos, safeZone);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestSafeZone = safeZone;
            }
        }

        return nearestSafeZone;
    }

    public bool IsInSafeZone(Vector2 pos) 
    {
        bool safeZone = true;

        foreach (Vector2 dangerZone in _dangerZones)
        {
            if (Vector2.Distance(dangerZone, pos) < 0.5f) 
            {
                safeZone = false;
                break;
            }
        }

        return safeZone;
    }

    public bool IsDangerZoneEmpty() 
    {
        return _dangerZones.Count == 0;
    }
}
