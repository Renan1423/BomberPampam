using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStatesEnum
{
    IDLE,
    DESTROY_OBSTACLES,
    DODGE,
    PURSUE,
    BOMB
}

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField]
    private EnemyStatesEnum _initialState = EnemyStatesEnum.IDLE;

    private EnemyState _currentState;
    private EnemyStatesEnum _currentStateEnum;
    [SerializeField]
    private SerializableDictionary<EnemyStatesEnum, EnemyState> _enemyStatesDictionary;
    private Dictionary<EnemyStatesEnum, EnemyState> _enemyStatesDict;

    #region COMPONENTS
    [SerializeField]
    private BomberCharacterController _characterController;
    #endregion

    private void Awake()
    {
        _enemyStatesDict = _enemyStatesDictionary.ToDictionary();
    }

    private void OnEnable()
    {
        Invoke(nameof(SetupDelegates), 0.1f);
        Invoke(nameof(StartEnemyState), 0.2f);
    }

    private void SetupDelegates() 
    {
        DangerZonesManager.instance.OnDangerZoneUpdated += OnDangerZoneUpdated;
    }

    private void OnDisable()
    {
        DangerZonesManager.instance.OnDangerZoneUpdated -= OnDangerZoneUpdated;
    }

    private void StartEnemyState() 
    {
        ForceStateChange(_initialState);
    }

    public void ChangeState(EnemyStatesEnum newState)
    {
        if (_currentStateEnum == newState)
            return;

        if (_currentState != null)
            _currentState.ExitState();

        EnemyStatesEnum previousState = _currentStateEnum;
        Debug.Log(newState);

        _currentState = _enemyStatesDict[newState];
        _currentStateEnum = newState;
        _currentState.EnterState(previousState);
    }

    private void ForceStateChange(EnemyStatesEnum newState)
    {
        if (_currentState != null)
            _currentState.ExitState();

        EnemyStatesEnum previousState = _currentStateEnum;

        _currentState = _enemyStatesDict[newState];
        _currentStateEnum = newState;
        _currentState.EnterState(previousState);
    }

    private void OnDangerZoneUpdated() 
    {
        if (DangerZonesManager.instance.IsDangerZoneEmpty())
            return;

        if (DangerZonesManager.instance.IsInSafeZone(transform.position))
            return;

        ChangeState(EnemyStatesEnum.DODGE);
    }

    public void QuitState()
    {
        ForceStateChange(EnemyStatesEnum.IDLE);
    }

    public EnemyStatesEnum GetCurrentState()
    {
        return _currentStateEnum;
    }
}
