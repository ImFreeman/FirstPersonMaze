using System;
using UnityEngine;
using Zenject;

public class CollectableView : MonoBehaviour
{
    public event EventHandler<int> OnPlayerEntered;

    private void OnTriggerEnter(Collider other)
    {
        OnPlayerEntered?.Invoke(this, other.gameObject.GetInstanceID());
    }

    public class Pool : MonoMemoryPool<CollectableView>
    {
    }
}