using Features.Input;
using System;

public class ApplicationnLauncher : IDisposable
{

    public ApplicationnLauncher(
        ICameraRotationInput rotationInput,
        IMovementInput movementInput,
        GameFlow gameFlow
        )
    {      
        rotationInput.IsActive = true;
        movementInput.IsActive = true;

        gameFlow.RestartGame();
    }

    public void Dispose()
    {
        
    }
}