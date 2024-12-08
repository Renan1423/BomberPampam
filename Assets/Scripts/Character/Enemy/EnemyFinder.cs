using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinder : MonoBehaviour
{
    [SerializeField]
    private float _enemyFinderRadius;

    public Transform FindClosestEnemy(BomberCharacterController character) 
    {
        Transform closestEnemy = null;
        float closestEnemyDist = float.MaxValue;

        BomberCharacterController[] enemies = FindObjectsOfType<BomberCharacterController>();

        foreach (BomberCharacterController enemy in enemies)
        {
            if (enemy == character)
                continue;

            float dist = Vector2.Distance(transform.position, enemy.transform.position);

            if (dist > _enemyFinderRadius)
                continue;

            if (dist < closestEnemyDist) 
            {
                closestEnemy = enemy.transform;
                closestEnemyDist = dist;
            }
        }

        return closestEnemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _enemyFinderRadius);
    }
}
