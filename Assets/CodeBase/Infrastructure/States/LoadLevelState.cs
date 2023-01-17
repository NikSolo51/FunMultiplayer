using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.PlayerScripts;
using CodeBase.Services.Audio;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayLoadedState<string>
    {
        private GameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;
        private LoadingCurtain _curtain;
        private IGameFactory _gameFactory;
        private IStaticDataService _staticData;
        public ISaveLoadService _saveLoadService;
        private DiContainer _container;


        public LoadLevelState(GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            LoadingCurtain curtain,
            IGameFactory gameFactory,
            IStaticDataService staticData,
            ISaveLoadService saveLoadService, DiContainer container)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _staticData = staticData;
            _saveLoadService = saveLoadService;
            _container = container;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _gameFactory.CleanUp();
            _gameFactory.WarmUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _curtain.Hide();
        }

        private async void OnLoaded()
        {
            await InitGameWorld();
            _saveLoadService.InformProgressReaders();
            _stateMachine.Enter<GameLoopState>();
        }


        private async Task InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();

            if (!levelData)
                return;
            if (!levelData.InitGameWorld)
                return;

        }

        private async void CreateUpdateManager()
        {
            await _gameFactory.CreateUpdateManager();
        }

        private async Task<ISoundService> CreateAudio(LevelStaticData levelData)
        {
            ISoundService soundManager = await _gameFactory.CreateSoundManager(levelData.SoundManagerData);
            _container.Bind<ISoundService>().FromInstance(soundManager).AsSingle().NonLazy();
            return soundManager;
        }


        private async Task<GameObject> CreateHero(LevelStaticData levelData)
        {
            GameObject hero = await _gameFactory.CreateHero(levelData.InitialHostPosition);
            HeroMove heroMove = hero.GetComponent<HeroMove>();
            _container.Inject(heroMove);
            return hero;
        }

        private async Task<GameObject> CreateCamera()
        {
            GameObject camera = await _gameFactory.CreateCamera();

            return camera;
        }

        private async Task<GameObject> CreateHud()
        {
            GameObject hud = await _gameFactory.CreateHud();
            return hud;
        }

        private LevelStaticData LevelStaticData() => _staticData.ForLevel(SceneManager.GetActiveScene().name);
    }
}