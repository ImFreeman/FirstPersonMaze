using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyView : MonoBehaviour
{
    public event EventHandler<int> OnPlayerContact;

    public NavMeshAgent Agent => _agent;
    public float DetectionRadius => _detectionRadius;
    public float LossRadius => _lossRadius;
    public LayerMask ObstacleMask => _obstacleMask;
    public Transform BodyTransform => _bodyTransform;

    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _detectionRadius = 10f;
    [SerializeField] private float _lossRadius = 15f;
    [SerializeField] private LayerMask _obstacleMask;

    public void WarpTo(Vector3 position)
    {        
        _agent.Warp(position);        
    }

    private void OnTriggerEnter(Collider other)
    {
        OnPlayerContact?.Invoke(this, other.gameObject.GetInstanceID());
    }    

    public class Pool : MonoMemoryPool<EnemyView>
    {

    }
}