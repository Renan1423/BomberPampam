using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    #region References
    [Header("References")]
    [SerializeField]
    private CharacterAttributes _charAttributes;
    [SerializeField]
    private CharacterAnimations _charAnimations;
    [SerializeField]
    private Rigidbody2D _rig;
    #endregion

    public void MoveCharacter(Vector2 moveDir) 
    {
        _rig.velocity = moveDir * _charAttributes.Attributes.Speed;
        if(moveDir != Vector2.zero)
            _charAnimations.SetCharacterDirection(moveDir);
    }
}
