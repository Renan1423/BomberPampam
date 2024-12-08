using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator _anim;

    public void SetCharacterDirection(Vector2 dir) 
    {
        _anim.SetFloat("DirX", dir.x);
        _anim.SetFloat("DirY", dir.y);
    }

    public void TriggerDeathAnimation() 
    { 
        
    }
}
