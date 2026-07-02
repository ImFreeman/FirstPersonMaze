using Features.Input;
using UnityEngine;

public interface IMovementInput : IInputElement
{
    public bool Sprint {  get; }
    public Vector2 Movement { get; }
}