using System;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : IDisposable
{
    private readonly CollectableView.Pool _pool;
    private readonly int _playerInstanceId;
    private readonly List<CollectableView> _spawnedCollectables = new List<CollectableView>();

    public event Action<int> OnCountChanged;
    public int Count => _spawnedCollectables.Count;
    public Collectables(CollectableView.Pool pool, PlayerView playerView)
    {
        _pool = pool;
        _playerInstanceId = playerView.gameObject.GetInstanceID();
    }

    public void Dispose()
    {
        if(_spawnedCollectables.Count > 0)
        {
            ClearAll();
        }    
    }

    public void SpawnCollectables(IEnumerable<Vector3> positions)
    {
        foreach (var position in positions)
        {
            var collectable = _pool.Spawn();
            collectable.transform.position = position;
            collectable.OnPlayerEntered += HandlePlayerEntered;
            _spawnedCollectables.Add(collectable);
        }

        OnCountChanged?.Invoke(_spawnedCollectables.Count);
    }

    public void ClearAll()
    {
        foreach (var collectable in _spawnedCollectables)
        {
            collectable.OnPlayerEntered -= HandlePlayerEntered;
            _pool.Despawn(collectable);
        }

        _spawnedCollectables.Clear();
        OnCountChanged?.Invoke(0);
    }

    private void HandlePlayerEntered(object sender, int instanceId)
    {
        if (instanceId != _playerInstanceId)
            return;

        var view = sender as CollectableView;
        if (view == null)
            return;

        view.OnPlayerEntered -= HandlePlayerEntered;
        _spawnedCollectables.Remove(view);
        _pool.Despawn(view);

        OnCountChanged?.Invoke(_spawnedCollectables.Count);
    }
    
}