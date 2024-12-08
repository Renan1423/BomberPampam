using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHitable : MonoBehaviour, IHitable
{
    [SerializeField]
    private CharacterAttributes _charAttributes;
    [SerializeField]
    private CharacterAnimations _charAnimations;
    private int _hp;

    private void Start()
    {
        _hp = _charAttributes.Attributes.HealthPoints; 
    }

    public void OnHit(GameObject hitOwner)
    {
        _hp--;

        if (_hp <= 0) 
        {
            _charAnimations.TriggerDeathAnimation();
            Destroy(gameObject);
        }
    }

    public void OnDestroyHitable()
    {

    }
}
