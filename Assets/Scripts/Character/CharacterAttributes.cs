using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    [field: SerializeField]
    public CharacterAttributesScriptable Attributes { get; private set; }
}
