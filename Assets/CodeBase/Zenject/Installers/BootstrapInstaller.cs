using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomaizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.Services.Update;
using CodeBase.WeaponsInventory;
using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [Inject]
    private DiContainer _container;
    public override void InstallBindings()
    {
        Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle().NonLazy();
        Container.Bind<GameStateMachine>().AsSingle().NonLazy();

        RegisterStaticData();
        RegisterAssetProvider();

        Container.Bind<IRandomService>().To<RandomService>().AsSingle().NonLazy();
        
        Container.Bind<IPlayerWeaponsInventory>().To<PlayerWeaponsInventory>().AsSingle().NonLazy();

        Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle().NonLazy();

        Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle().NonLazy();
        
        Container.Bind<IUpdateService>().To<UpdateManager>().AsSingle().NonLazy();
        Container.Bind<IGameFactory>().To<GameFactory>().AsSingle().WithArguments(
            Container.Resolve<IAssets>(),
            Container.Resolve<IStaticDataService>(),
            Container.Resolve<ISaveLoadService>(),
            _container
            ).NonLazy();
    }

    private void RegisterAssetProvider()
    {
        AssetsProvider assetsProvider = new AssetsProvider();
        assetsProvider.Initialize();
        Container.Bind<IAssets>().To<AssetsProvider>().FromInstance(assetsProvider).AsSingle().NonLazy();
    }

    private void RegisterStaticData()
    {
        StaticDataService staticDataService = new StaticDataService();
        staticDataService.Initialize();
        Container.Bind<IStaticDataService>().To<StaticDataService>().FromInstance(staticDataService).AsSingle().NonLazy();
    }

    private IInputService SetupMovementInputService()
    {
        return new StandaloneInputService();
    }
}