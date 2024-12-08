using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongPositionChecker : MonoBehaviour
{
    [SerializeField]
    private ObstacleHitable _obstacle;
    [SerializeField]
    private Collider2D _obstacleCollider;

    private void OnEnable()
    {
        StartCoroutine(CheckCollisionWithPlayer());
    }

    private IEnumerator CheckCollisionWithPlayer() 
    {
        yield return new WaitForSeconds(0.1f);

        _obstacleCollider.enabled = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check collision with player
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 8) 
        {
            StopAllCoroutines();
            _obstacle.OnDestroyHitable();
        }
    }
}
