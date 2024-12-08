using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyObstaclesState : EnemyState
{
    private Vector2 _targetPos;
    [SerializeField]
    private Transform _targetShowPoint;

    public override void EnterState(EnemyStatesEnum previousState)
    {
        base.EnterState(previousState);

        _targetShowPoint.transform.SetParent(null);
        _targetShowPoint.position = _targetPos;
        _enemyController.SetNewDestination(_targetPos, OnReachedDestination);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private void OnReachedDestination() 
    {
        _enemyStateMachine.ChangeState(EnemyStatesEnum.BOMB);
    }

    public void SetTarget(Vector2 target) 
    {
        _targetPos = target;
    }
}
