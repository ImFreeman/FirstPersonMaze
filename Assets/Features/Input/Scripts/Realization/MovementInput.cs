using Features.Input;
using UnityEngine;
using Zenject;

public class MovementInput : InputElement, IMovementInput
{
    private readonly KeyCode _sprintKey;
    private readonly KeyCode _leftKey;
    private readonly KeyCode _rightKey;
    private readonly KeyCode _forwardKey;
    private readonly KeyCode _backwardKey;

    private bool _isSprinting;
    public Vector2 _movement;    
    public MovementInput(TickableManager tickableManager, PlayerConfig config) : base(tickableManager)
    {
        _sprintKey = config.SprintKey;
        _leftKey = config.LeftKey;
        _rightKey = config.RightKey;
        _forwardKey = config.ForwardKey;
        _backwardKey = config.BackwardKey;
    }

    public bool Sprint => _isSprinting;

    public Vector2 Movement => _movement;

    public override void Tick()
    {
        _isSprinting = Input.GetKey(_sprintKey);

        float horizontal = (Input.GetKey(_rightKey) ? 1f : 0f) - (Input.GetKey(_leftKey) ? 1f : 0f);
        float vertical = (Input.GetKey(_forwardKey) ? 1f : 0f) - (Input.GetKey(_backwardKey) ? 1f : 0f);

        _movement = new Vector2(horizontal, vertical);        
    }
}