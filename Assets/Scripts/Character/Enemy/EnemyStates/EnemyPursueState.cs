using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPursueState : EnemyState
{
    private Transform _targetTrans;
    [SerializeField]
    private Transform _targetShowPoint;
    [SerializeField]
    private float _minDistanceToDropBomb = 2f;

    public override void EnterState(EnemyStatesEnum previousState)
    {
        base.EnterState(previousState);

        _targetShowPoint.transform.SetParent(null);
        InvokeRepeating(nameof(PursueTarget), 0f, 0.75f);
    }

    public override void ExitState()
    {
        base.ExitState();
        _targetTrans = null;
        CancelInvoke(nameof(PursueTarget));
    }

    private void PursueTarget() 
    {
        if (_targetTrans == null)
            return;

        _targetShowPoint.position = _targetTrans.position;
        Vector2 targetPos = StageGridManager.instance.FindClosestFreeTileAdjacentToPosition(_targetTrans.position, _enemyController.transform.position);

        _enemyController.SetNewDestination(targetPos, OnReachedDestination);
    }
    private void OnReachedDestination()
    {
        _enemyStateMachine.ChangeState(EnemyStatesEnum.BOMB);
    }

    public void SetTargetTrans(Transform target)
    {
        _targetTrans = target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _minDistanceToDropBomb);
    }
}
