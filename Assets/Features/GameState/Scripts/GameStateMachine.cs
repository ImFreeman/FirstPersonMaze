public class GameStateMachine
{
    private IGameState _currentState;
    public void SetState(IGameState state)
    {
        if(_currentState != null)
        {
            _currentState.OnExit();
        }
        _currentState = state;
        _currentState.OnEntry();
    }
}