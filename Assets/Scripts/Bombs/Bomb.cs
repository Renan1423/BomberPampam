using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Bomb : ObjectFromPool, IHitable
{
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private Collider2D _col;
    private DangerZonesManager _dangerZonesManager;

    [SerializeField]
    private BombTimer _bombTimer;
    public int BombExplosionSize { get; private set;}
    [SerializeField]
    private float _timeToExplode = 2f;
    [SerializeField]
    private SerializableDictionary<UnityEvent, float> _timeEvents;

    [HideInInspector]
    public UnityEvent OnBombExplode;

    public override void OnObjectSpawn() 
    {
        _dangerZonesManager = DangerZonesManager.instance;
    }

    public void StartBomb(int bombExplosionSize, UnityAction onExplode) 
    {
        if(_dangerZonesManager == null)
            _dangerZonesManager = DangerZonesManager.instance;

        OnBombExplode.RemoveAllListeners();
        OnBombExplode.AddListener(onExplode);

        BombExplosionSize = bombExplosionSize;
        _bombTimer.SetupTimer(_timeToExplode, Explode, _timeEvents.ToDictionary());

        GameObject tilemapGO = GameObject.Find("WallTilemap");
        Tilemap tilemap = tilemapGO.GetComponent<Tilemap>();

        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);

        // Convert the cell position back to world position (center of the cell)
        Vector3 cellWorldPosition = tilemap.GetCellCenterWorld(cellPosition);

        // Set the bomb's position to the center of the closest cell
        transform.position = cellWorldPosition;
        //Debug.Log(transform.position);

        _dangerZonesManager.UpdateDangerZone();
    }

    private void Explode() 
    {
        Vector2[] directions = {
                Vector2.up,
                Vector2.down,
                Vector2.left,
                Vector2.right
            };

        Dictionary<Vector2, Explosion> explosionChain = new Dictionary<Vector2, Explosion>();

        for (int i = 0; i <= BombExplosionSize; i++)
        {
            if (i == 0) 
            {
                GameObject explosionGO = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Explosion explosion = explosionGO.GetComponent<Explosion>();
                explosion.ExplosionUnlocked = true;
                continue;
            }

            foreach (Vector2 dir in directions)
            {
                Vector2 explosionPosition = (Vector2) transform.position + (dir * i);

                GameObject explosionGO = Instantiate(_explosionPrefab, explosionPosition, Quaternion.identity);
                Explosion explosion = explosionGO.GetComponent<Explosion>();
                if (explosionChain.ContainsKey(dir))
                {
                    explosionChain[dir].ChainExplosion(explosion);
                    explosionChain[dir] = explosion;
                }
                else 
                {
                    explosionChain.Add(dir, explosion);
                    explosion.ExplosionUnlocked = true;
                }
            }
        }

        OnBombExplode?.Invoke();
        Unspawn();
    }

    public void OnHit(GameObject hitOwner)
    {
        _bombTimer.StopTimer();
        Explode();
    }

    public override void Unspawn()
    {
        BombExplosionSize = 0;
        _dangerZonesManager.UpdateDangerZone();
        base.Unspawn();
    }

    public void OnDestroyHitable()
    {

    }
}
