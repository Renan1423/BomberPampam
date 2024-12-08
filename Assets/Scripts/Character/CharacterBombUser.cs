using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBombUser : MonoBehaviour
{
    [SerializeField]
    private CharacterAttributes _charAttributes;
    [SerializeField]
    private Transform _bombSpawnPoint;
    public int BombsOnField { get; private set; }

    public void PlaceBomb() 
    {
        if (BombsOnField >= _charAttributes.Attributes.BombsAmount)
            return;

        BombsOnField++;

        GameObject bombGO = ObjectPooler.instance.SpawnFromPool(_charAttributes.Attributes.BombPrefabTag, _bombSpawnPoint.position, Quaternion.identity);
        Bomb bomb = bombGO.GetComponent<Bomb>();

        bomb.StartBomb(_charAttributes.Attributes.BombExplosionSize, OnBombExplode);
    }

    private void OnBombExplode() 
    {
        BombsOnField--;
    }
}
