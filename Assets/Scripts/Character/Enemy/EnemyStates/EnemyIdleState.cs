using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    [SerializeField]
    private EnemyFinder _enemyFinder;
    [SerializeField]
    private CharacterBombUser _enemyBombUser;
    [SerializeField]
    private CharacterAttributes _enemyAttributes;
    [SerializeField]
    private EnemyDestroyObstaclesState _enemyDestroyObstaclesState;
    [SerializeField]
    private EnemyPursueState _enemyPursueState;

    public override void EnterState(EnemyStatesEnum previousState)
    {
        base.EnterState(previousState);
        _enemyController.StopMovement();

        StartCoroutine(MakeDecision());
    }

    private IEnumerator MakeDecision() 
    {
        //yield return new WaitUntil(() => _enemyBombUser.BombsOnField < _enemyAttributes.Attributes.BombsAmount);

        yield return new WaitForSeconds(1.5f);

        Transform closestEnemyTrans = _enemyFinder.FindClosestEnemy(_enemyController);
        if (closestEnemyTrans != null)
        {            
            _enemyPursueState.SetTargetTrans(closestEnemyTrans);
            _enemyStateMachine.ChangeState(EnemyStatesEnum.PURSUE);
        }
        else 
        {
            Vector2 obtaclePos = StageGridManager.instance.FindClosestObstacle(_enemyController.transform.position);
            Vector2 targetPos = StageGridManager.instance.FindClosestFreeTileAdjacentToPosition(obtaclePos, _enemyController.transform.position);
            _enemyDestroyObstaclesState.SetTarget(targetPos);
            _enemyStateMachine.ChangeState(EnemyStatesEnum.DESTROY_OBSTACLES);
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        StopAllCoroutines();
    }
}
