using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDodgeState : EnemyState
{
    [SerializeField]
    private Transform _targetShowPoint;

    public override void EnterState(EnemyStatesEnum previousState)
    {
        base.EnterState(previousState);

        StartCoroutine(ForceExitState());

        if (DangerZonesManager.instance.IsInSafeZone(transform.position)) 
        {
            _enemyStateMachine.ChangeState(EnemyStatesEnum.IDLE);
            return;
        }

        _targetShowPoint.transform.SetParent(null);

        Vector2 targetPos = DangerZonesManager.instance.FindNearestSafeZone(_enemyController.transform.position);
        _targetShowPoint.position = targetPos;
        _enemyController.SetNewDestination(targetPos, OnReachedDestination);
    }

    private IEnumerator ForceExitState() 
    {
        yield return new WaitForSeconds(2f);

        _enemyStateMachine.ChangeState(EnemyStatesEnum.IDLE);
    }

    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }

    private void OnReachedDestination() 
    {
        _enemyStateMachine.ChangeState(EnemyStatesEnum.IDLE);
    }
}
