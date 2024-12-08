using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    [SerializeField]
    protected EnemyController _enemyController;
    [SerializeField]
    protected EnemyStateMachine _enemyStateMachine;

    public virtual void EnterState(EnemyStatesEnum previousState) { }
    public virtual void ExitState() { }
}
