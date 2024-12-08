using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : BomberCharacterController
{
    [SerializeField]
    private Seeker _seeker;

    private Path _path;
    private int _currentWaypoint = 0;
    [SerializeField]
    private float _nextWaypointDistance = 1f;
    [SerializeField]
    private float _lastWaypointSmoothSpeed = 10f;

    private UnityEvent OnReachedDestination = new UnityEvent();

    public bool SetNewDestination(Vector2 targetPosition, UnityAction callback)
    {
        _path = null;

        OnReachedDestination.RemoveAllListeners();
        OnReachedDestination.AddListener(callback);
        StopAllCoroutines();
        _seeker.StartPath(transform.position, targetPosition, OnPathCompleted);

        return _path != null;
    }

    private void OnPathCompleted(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    private void Update()
    {
        if (_path == null)
            return;

        float distance = Vector2.Distance(transform.position, _path.vectorPath[_currentWaypoint]);

        if (distance < _nextWaypointDistance)
        {
            _currentWaypoint++;

            if (_currentWaypoint >= _path.vectorPath.Count)
            {
                //transform.position = _path.vectorPath[_currentWaypoint - 1];
                StopMovement();
                OnReachedDestination?.Invoke();
                return;
            }

            Vector2 dir = (_path.vectorPath[_currentWaypoint] - transform.position).normalized;
            int dirX = Mathf.RoundToInt(dir.x);
            int dirY = Mathf.RoundToInt(dir.y);
            MoveCharacter(new Vector2(dirX, dirY));
        }
    }

    public void StopMovement()
    {
        _path = null;
        _currentWaypoint = 0;
        MoveCharacter(Vector2.zero);
    }
}
