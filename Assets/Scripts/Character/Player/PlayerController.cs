using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BomberCharacterController
{
    private float _currentDirX;
    private float _currentDirY;

    public void MovePlayerHorizontal(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();

        _currentDirX = input.x;

        if (input == Vector2.zero && _currentDirY != 0)
            return;

        _currentDirY = 0f;
        MoveCharacter(input);
    }

    public void MovePlayerVertical(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();

        _currentDirY = input.y;

        if (input == Vector2.zero && _currentDirX != 0)
            return;

        _currentDirX = 0f;
        MoveCharacter(input);
    }

    public void PlaceBomb(InputAction.CallbackContext value) 
    {
        if (value.performed) 
        {
            PlaceCharacterBomb();
        }
    }
}
