using System;
using UnityEngine;
using Zenject;

public class PlayerMovement : IDisposable, ITickable
{
    private readonly TickableManager _tickableManager;
    private PlayerView _view;
    private readonly IMovementInput _movementInput;
    private readonly float _normalSpeed;
    private readonly float _sprintSpeed;

    private bool _isActive;
    public PlayerMovement(
            TickableManager tickableManager,
            IMovementInput movementInput,
            PlayerConfig config,
            PlayerView view
            )
    {
        _movementInput = movementInput;
        _tickableManager = tickableManager;        
        _view = view;
        _normalSpeed = config.Speed;
        _sprintSpeed = config.SprintSpeed;
    }
    public void Dispose()
    {
        if(_isActive)
        {
            SetActive(false);
        }
    }

    public void SetActive(bool value)
    {
        if (value != _isActive)
        {
            if (value)
            {
                _tickableManager.Add(this);
            }
            else
            {
                _tickableManager.Remove(this);
            }
            _isActive = value;
        }
    }

    public void Tick()
    {
        float targetSpeed = _movementInput.Sprint ? _sprintSpeed : _normalSpeed;
        
        if (_movementInput.Movement == Vector2.zero) targetSpeed = 0.0f;

        Vector3 inputDirection = new Vector3(_movementInput.Movement.x, 0.0f, _movementInput.Movement.y).normalized;

        if (_movementInput.Movement != Vector2.zero)
        {
            inputDirection = _view.BodyTransform.right 
                * _movementInput.Movement.x 
                + _view.BodyTransform.forward 
                * _movementInput.Movement.y;
        }
        _view.CharacterController.Move(inputDirection.normalized * targetSpeed * Time.deltaTime);
    }
}