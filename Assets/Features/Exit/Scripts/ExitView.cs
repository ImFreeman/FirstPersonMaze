using System;
using UnityEngine;

public class ExitView : MonoBehaviour
{
    public event EventHandler<int> OnPlayerEntered;

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _openMaterial;
    [SerializeField] private Material _closedMaterial;    

    private void OnTriggerEnter(Collider other)
    {
        OnPlayerEntered?.Invoke(this, other.gameObject.GetInstanceID());
    }

    public void SetOpen(bool value)
    {        
        _meshRenderer.material = value ? _openMaterial : _closedMaterial;
    }
}