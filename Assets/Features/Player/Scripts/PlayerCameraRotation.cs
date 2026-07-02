using Features.Input;
using UnityEngine;
using Zenject;

public class PlayerCameraRotation : ITickable
{
    private readonly TickableManager _tickableManager;
    private readonly float _sensivity;
    private readonly PlayerView _playerView;
    private readonly ICameraRotationInput _cameraRotationInput;

    private bool _isActive;
    private float _xRotation = 0f;

    public PlayerCameraRotation(
        TickableManager tickableManager,
        ICameraRotationInput cameraRotationInput,
        PlayerConfig config,
        PlayerView playerView
        )
    {
        _cameraRotationInput = cameraRotationInput;
        _tickableManager = tickableManager;
        _sensivity = config.Sensivity;
        _playerView = playerView;
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
        float mouseX = _cameraRotationInput.InputX * _sensivity * Time.deltaTime;
        float mouseY = _cameraRotationInput.InputY * _sensivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _playerView.MainCameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerView.BodyTransform.Rotate(Vector3.up * mouseX);
    }
}