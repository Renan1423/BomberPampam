using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBombState : EnemyState
{
    public override void EnterState(EnemyStatesEnum previousState)
    {
        base.EnterState(previousState);

        _enemyController.PlaceCharacterBomb();
        _enemyStateMachine.ChangeState(EnemyStatesEnum.IDLE);
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
