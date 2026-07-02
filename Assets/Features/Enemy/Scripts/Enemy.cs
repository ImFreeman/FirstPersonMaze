using System;
using UnityEngine;
using Zenject;

public class Enemy : IDisposable, ITickable
{
    private readonly EnemyView _view;
    private readonly PlayerView _playerView;
    private readonly Vector3[] _patrolPoints;
    private readonly TickableManager _tickableManager;

    private int _currentPointIndex;
    private bool _isChasing;
    private bool _isInitialized;


    public Enemy(EnemyView view, PlayerView playerView, Vector3[] patrolPoints, TickableManager tickableManager)
    {
        _view = view;
        _playerView = playerView;
        _patrolPoints = patrolPoints;
        _tickableManager = tickableManager;
        _currentPointIndex = 0;
        _isChasing = false;
        _isInitialized = false;        

        _tickableManager.Add(this);
    }

    public void Dispose()
    {        
        _tickableManager.Remove(this);
    }

    public void Tick()
    {
        if (!_isInitialized)
        {
            if (_patrolPoints.Length > 0)
            {                
                _view.Agent.SetDestination(_patrolPoints[0]);                
            }
            _isInitialized = true;
        }
        if (_isChasing)
        {
            UpdateChase();
        }
        else
        {
            UpdatePatrol();
        }
    }

    private void UpdatePatrol()
    {
        if (!_view.Agent.pathPending && _view.Agent.remainingDistance <= 0.5f)
        {
            _currentPointIndex = (_currentPointIndex + 1) % _patrolPoints.Length;
            _view.Agent.SetDestination(_patrolPoints[_currentPointIndex]);
        }

        if (IsPlayerDetected())
        {
            _isChasing = true;
        }
    }

    private void UpdateChase()
    {
        var playerPosition = _playerView.BodyTransform.position;
        _view.Agent.SetDestination(playerPosition);

        var distance = Vector3.Distance(_view.BodyTransform.position, playerPosition);
        if (distance > _view.LossRadius)
        {
            _isChasing = false;
            _view.Agent.SetDestination(_patrolPoints[_currentPointIndex]);
        }
    }

    private bool IsPlayerDetected()
    {
        var playerPosition = _playerView.BodyTransform.position;
        var distance = Vector3.Distance(_view.BodyTransform.position, playerPosition);

        if (distance > _view.DetectionRadius)
        {
            Debug.Log($"big radius {_view.name}");
            return false;
        }
            

        if (Physics.Linecast(_view.BodyTransform.position, playerPosition, _view.ObstacleMask))
        {
            Debug.Log($"obstacle {_view.name}");
            return false;
        }            

        return true;
    }    
}