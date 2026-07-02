using Features.Player;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState
{
    public void OnEntry();
    public void OnExit();

}

public class MainGameState : IGameState
{
    private readonly Collectables _collectables;
    private readonly IUIService _uiService;
    private readonly IEnumerable<Vector3> _collectablesSpawnPoints;
    private readonly Vector3 _playerSpawnPoint;
    private readonly PlayerView _playerView;
    private readonly PlayerCameraRotation _cameraRotation;
    private readonly PlayerMovement _movement;
    private readonly Enemies _enemies;
    private readonly IEnumerable<Vector3> _enemySpawnPositions;
    private readonly Vector3[][] _enemiesPatrolRoutes;

    public MainGameState(
        PlayerView playerView,        
        IUIService uiService,
        Collectables collectables,
        PlayerCameraRotation playerCamera,
        PlayerMovement playerMovement,
        Enemies enemies,
        Vector3 playerSpawnPoint,
        IEnumerable<Vector3> collectablesSpawnPoints,
        IEnumerable<Vector3> enemySpawnPositions,
        Vector3[][] enemiesPatrolRoutes)
    {
        _playerView = playerView;
        _playerSpawnPoint = playerSpawnPoint;
        _collectablesSpawnPoints = collectablesSpawnPoints;
        _uiService = uiService;
        _collectables = collectables;
        _cameraRotation = playerCamera;
        _movement = playerMovement;
        _enemies = enemies;
        _enemySpawnPositions = enemySpawnPositions;
        _enemiesPatrolRoutes = enemiesPatrolRoutes;
    }
        
    public void OnEntry()
    {        
        _uiService.Show<UIHudWindow>();        
        _collectables.ClearAll();
        _collectables.SpawnCollectables(_collectablesSpawnPoints);
        _uiService.Get<UIHudWindow>().ScoreText.text = _collectables.Count.ToString();
        _playerView.SetPositionTo(_playerSpawnPoint);
        _enemies.SpawnEnemies(_enemySpawnPositions, _enemiesPatrolRoutes);
        _cameraRotation.SetActive(true);
        _movement.SetActive(true);
    }

    public void OnExit()
    {
        _uiService.Hide<UIHudWindow>();
        _enemies.ClearAll();
        _cameraRotation.SetActive(false);
        _movement.SetActive(false);
    }
}

public class GameOverState : IGameState
{
    private readonly IUIService _uiService;    

    public GameOverState(IUIService uiService)
    {
        _uiService = uiService;
    }

    public void OnEntry()
    {
        _uiService.Show<UIGameOverWindow>();
    }

    public void OnExit()
    {
        _uiService.Hide<UIGameOverWindow>();
    }
}

public class VictoryGameState : IGameState
{
    private readonly IUIService _uiService;

    public VictoryGameState(IUIService uiService)
    {
        _uiService = uiService;
    }

    public void OnEntry()
    {
        _uiService.Show<UIVictoryWindow>();
    }

    public void OnExit()
    {
        _uiService.Hide<UIVictoryWindow>();
    }
}
