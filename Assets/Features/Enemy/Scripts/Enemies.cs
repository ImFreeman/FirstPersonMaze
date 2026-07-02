using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Enemies : IDisposable
{
    private readonly EnemyView.Pool _pool;
    private readonly IInstantiator _instantiator;
    private readonly PlayerView _playerView;
    private readonly List<EnemyView> _spawnedViews = new List<EnemyView>();
    private readonly List<Enemy> _spawnedEnemies = new List<Enemy>();

    public event Action<int> OnCountChanged;
    public event Action OnPlayerContact;

    public Enemies(EnemyView.Pool pool, IInstantiator instantiator, PlayerView playerView)
    {
        _pool = pool;
        _instantiator = instantiator;
        _playerView = playerView;
    }

    public void Dispose()
    {
        if(_spawnedEnemies.Count > 0)
        {
            ClearAll();
        }
    }

    public void SpawnEnemies(IEnumerable<Vector3> positions, Vector3[][] patrolRoutes)
    {
        var positionList = new List<Vector3>(positions);
        var routeList = new List<Vector3[]>(patrolRoutes);

        for (int i = 0; i < positionList.Count; i++)
        {
            var enemyView = _pool.Spawn();
            enemyView.WarpTo(positionList[i]);            

            var patrolPoints = i < routeList.Count ? routeList[i] : new Vector3[0];
            var enemy = _instantiator.Instantiate<Enemy>(new object[] { enemyView, _playerView, patrolPoints });

            enemyView.OnPlayerContact += HandlePlayerContact;

            _spawnedViews.Add(enemyView);
            _spawnedEnemies.Add(enemy);
        }

        OnCountChanged?.Invoke(_spawnedEnemies.Count);
    }

    public void ClearAll()
    {
        foreach (var enemyView in _spawnedViews)
        {
            enemyView.OnPlayerContact -= HandlePlayerContact;                        
            _pool.Despawn(enemyView);
        }

        foreach (var enemy in _spawnedEnemies)
        {
            enemy.Dispose();
        }

        _spawnedViews.Clear();
        _spawnedEnemies.Clear();
        OnCountChanged?.Invoke(0);
    }

    private void HandlePlayerContact(object sender, int instanceId)
    {
        if (instanceId != _playerView.gameObject.GetInstanceID())
            return;

        OnPlayerContact?.Invoke();
    }    
}