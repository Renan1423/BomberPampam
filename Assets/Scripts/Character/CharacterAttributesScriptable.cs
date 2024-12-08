using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAttribute", menuName = "Characters/Attributes")]
public class CharacterAttributesScriptable : ScriptableObject
{
    [field: SerializeField]
    public int HealthPoints { get; private set; }
    [field: SerializeField]
    public float Speed { get; private set; }
    [field: SerializeField]
    public string BombPrefabTag { get; private set; }
    [field: SerializeField]
    public int BombExplosionSize { get; private set; }
    [field: SerializeField]
    public float BombsAmount { get; private set; }
}
