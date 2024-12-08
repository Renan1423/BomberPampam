using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberCharacterController : MonoBehaviour
{
    #region References
    [SerializeField]
    protected CharacterMovement _CharMovement;
    [SerializeField]
    protected CharacterBombUser _CharBombUser;
    #endregion

    public void MoveCharacter(Vector2 dir) 
    {
        _CharMovement.MoveCharacter(dir);
    }

    public void PlaceCharacterBomb() 
    {
        _CharBombUser.PlaceBomb();
    }
}
