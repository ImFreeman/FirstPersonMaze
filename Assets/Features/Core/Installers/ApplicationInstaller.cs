using Features.Input;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ApplicationInstaller : MonoInstaller
{
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private RectTransform _activeContainerUI;
    [SerializeField] private CollectableView _collectableViewPrefab;
    [SerializeField] private Transform[] _coinPoints;
    [SerializeField] private ExitView _exitView;
    [SerializeField] private EnemyView _enemyViewPrefab;
    [SerializeField] private EnemyRoute[] _enemyRoutes;


    public override void InstallBindings()
    {
        Container
            .Bind<IUIService>()
            .To<UIService>()
            .AsSingle()
            .WithArguments(_activeContainerUI)
            .NonLazy();

        Container
            .BindMemoryPool<CollectableView, CollectableView.Pool>()
            .FromComponentInNewPrefab(_collectableViewPrefab);            

        Container
            .Bind<Collectables>()
            .AsSingle();        

        Container
            .Bind<PlayerConfig>()
            .FromScriptableObject(_playerConfig)
            .AsSingle();

        Container
            .BindInstance(_playerView)
            .AsSingle();

        Container
            .Bind<ICameraRotationInput>()
            .To<MousePositionInput>()
            .AsSingle();

        Container
            .Bind<IMovementInput>()
            .To<MovementInput>()
            .AsSingle();

        Container
            .Bind<PlayerCameraRotation>()
            .AsSingle();

        Container
            .Bind<PlayerMovement>()
            .AsSingle();

        Container
            .Bind<GameStateMachine>()
            .AsSingle();

        var cPoints = new List<Vector3>();
        foreach (var coinPoint in _coinPoints)
        {
            cPoints.Add(coinPoint.position);
        }

        var spawnPositions = new Vector3[_enemyRoutes.Length];
        var patrolRoutes = new Vector3[_enemyRoutes.Length][];

        for (int i = 0; i < _enemyRoutes.Length; i++)
        {
            spawnPositions[i] = _enemyRoutes[i].SpawnPoint.position;

            patrolRoutes[i] = new Vector3[_enemyRoutes[i].PathRoute.Length];
            for (int j = 0; j < _enemyRoutes[i].PathRoute.Length; j++)
            {
                patrolRoutes[i][j] = _enemyRoutes[i].PathRoute[j].position;
            }
        }

        Container
            .Bind<GameFlow>()
            .AsSingle()
            .WithArguments(_playerSpawnPoint.position, cPoints, spawnPositions, patrolRoutes);

        Container
            .Bind<Hud>()
            .AsSingle()
            .NonLazy();

        Container
            .BindInstance(_exitView)
            .AsSingle();

        Container
            .Bind<ExitController>()
            .AsSingle();

        Container
            .BindMemoryPool<EnemyView, EnemyView.Pool>()
            .FromComponentInNewPrefab(_enemyViewPrefab);

        Container
            .Bind<Enemies>()
            .AsSingle();

        Container
            .Bind<ApplicationnLauncher>()
            .AsSingle()
            .NonLazy();
    }

    [Serializable]
    public struct EnemyRoute
    {
        public Transform SpawnPoint;
        public Transform[] PathRoute;
    }
}
