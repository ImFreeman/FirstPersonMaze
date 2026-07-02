using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameFlow : IDisposable
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly IGameState _mainGameState;
    private readonly IGameState _gameOverState;
    private readonly IGameState _victoryState;

    private readonly ExitController _exitController;
    private readonly Enemies _enemies;
    private readonly UIGameOverWindow _gameOverWindow;
    private readonly UIVictoryWindow _victoryWindow;

    public GameFlow(
        IInstantiator instantiator,
        GameStateMachine gameStateMachine,
        IUIService uiService,
        ExitController exitController,
        Enemies enemies,
        Vector3 playerSpawnPoint,
        IEnumerable<Vector3> collectablesSpawnPoints,
        IEnumerable<Vector3> enemySpawnPositions,
        Vector3[][] enemiesPatrolRoutes
        )
    {
        _gameStateMachine = gameStateMachine;
        _exitController = exitController;
        _enemies = enemies;
        _gameOverWindow = uiService.Get<UIGameOverWindow>();
        _victoryWindow = uiService.Get<UIVictoryWindow>();

        _mainGameState = instantiator.Instantiate<MainGameState>(new object[]
                {
                    playerSpawnPoint,
                    collectablesSpawnPoints,
                    enemySpawnPositions,
                    enemiesPatrolRoutes
                });
        _gameOverState = instantiator.Instantiate<GameOverState>();
        _victoryState = instantiator.Instantiate<VictoryGameState>();

        _exitController.OnPlayerExited += OnPlayerExited;
        _gameOverWindow.ButtonPressed += OnGameOverRestartButtonPressed;
        _victoryWindow.ButtonPressed += OnVictoryWindowButtonPressed;
        _enemies.OnPlayerContact += EnemyOnPlayerContact;
    }    

    public void Dispose()
    {
        _exitController.OnPlayerExited -= OnPlayerExited;
        _gameOverWindow.ButtonPressed -= OnGameOverRestartButtonPressed;
        _victoryWindow.ButtonPressed -= OnVictoryWindowButtonPressed;
    }

    public void RestartGame()
    {
        _gameStateMachine.SetState(_mainGameState);
    }    

    private void OnVictoryWindowButtonPressed(object sender, EventArgs e)
    {
        RestartGame();
    }
    private void OnGameOverRestartButtonPressed(object sender, EventArgs e)
    {
        RestartGame();
    }
    private void OnPlayerExited()
    {
        _gameStateMachine.SetState(_victoryState);
    }

    private void EnemyOnPlayerContact()
    {
        _gameStateMachine.SetState(_gameOverState);
    }
}