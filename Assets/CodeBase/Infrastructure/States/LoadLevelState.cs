using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.PlayerScripts;
using CodeBase.Services.Audio;
using CodeBase.Services.Input;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.Services.Update;
using CodeBase.UI;
using CodeBase.Weapons;
using CodeBase.Weapons.Reload;
using CodeBase.WeaponsInventory;
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
        private IPlayerWeaponsInventory _playerWeaponsInventory;


        public LoadLevelState(GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            LoadingCurtain curtain,
            IGameFactory gameFactory,
            IStaticDataService staticData,
            ISaveLoadService saveLoadService,
            IPlayerWeaponsInventory playerWeaponsInventory
            , DiContainer container)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _staticData = staticData;
            _saveLoadService = saveLoadService;
            _playerWeaponsInventory = playerWeaponsInventory;
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
            
            //CreateUpdateManager();

            GameObject player = CreatePlayer(levelData);
            GameObject cameraGO = await CreateCamera(levelData.InitialCameraPosition, levelData.InitialCameraRotation);
            Camera camera = cameraGO.GetComponent<Camera>();

            GameObject playerUi = await _gameFactory.CreatePlayerUI();
            HealthUIIndicator healthUiIndicator = playerUi.GetComponentInChildren<HealthUIIndicator>();
            ReloadUIIndicator reloadUiIndicator = playerUi.GetComponentInChildren<ReloadUIIndicator>();
            AmmoCountUI ammoCountUi = playerUi.GetComponentInChildren<AmmoCountUI>();

            PlayerGun playerGun = PlayerGun(player);
            GameObject weapon = await CreateWeapon(playerGun);
            PlayerWeapon playerWeapon = PlayerWeapon(weapon, reloadUiIndicator);

            AmmoCountUI(ammoCountUi, playerWeapon);

            FollowCamera(camera, player);
            HeroMove(player, camera);
            Health(player, healthUiIndicator);
            await CreateHud();
            await CreateAudio(levelData);
        }

        private static void Health(GameObject player, HealthUIIndicator healthUiIndicator)
        {
            player.GetComponent<IHealth>().OnHpPercent += healthUiIndicator.AnimateIndicator;
        }

        private static void AmmoCountUI(AmmoCountUI ammoCountUi, PlayerWeapon playerWeapon)
        {
            ammoCountUi.InitializeMaxAmmo(playerWeapon.MagazineCount);
            playerWeapon.OnAmmoCount += ammoCountUi.UpdateAmmoText;
        }

        private PlayerWeapon PlayerWeapon(GameObject weapon, ReloadUIIndicator reloadUiIndicator)
        {
            PlayerWeapon playerWeapon = weapon.GetComponent<PlayerWeapon>();
            playerWeapon.Construct(_gameFactory);
            playerWeapon.OnReloadPercent += reloadUiIndicator.AnimateIndicator;
            _playerWeaponsInventory.PlayerWeapon = playerWeapon;
            return playerWeapon;
        }

        private async Task<GameObject> CreateWeapon(PlayerGun playerGun)
        {
            GameObject weapon = await _gameFactory.CreateWeapon(_playerWeaponsInventory.WeaponType,
                playerGun._weaponInitPoint);

            weapon.transform.position = playerGun._weaponInitPoint.position;
            weapon.transform.SetParent(playerGun._weaponInitPoint);

            return weapon;
        }


        private void FollowCamera(Camera camera, GameObject player)
        {
            FollowCamera followCamera = camera.GetComponent<FollowCamera>();
            followCamera.Setup(player);
            _container.Inject(followCamera);
        }

        private PlayerGun PlayerGun(GameObject player)
        {
            PlayerGun playerGun = player.GetComponent<PlayerGun>();
            _container.Inject(playerGun);
            return playerGun;
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


        private GameObject CreatePlayer(LevelStaticData levelData)
        {
            Vector3 spawnPos = PhotonNetwork.IsMasterClient
                ? levelData.InitialHostPosition
                : levelData.InitialOthePlayerPosition;
            GameObject hero = _gameFactory.CreateHero(spawnPos);
            return hero;
        }

        private void HeroMove(GameObject hero, Camera camera)
        {
            HeroMove heroMove = hero.GetComponent<HeroMove>();
            _container.Inject(heroMove);
            heroMove.Setup(camera);
        }

        private async Task<GameObject> CreateCamera(Vector3 cameraInitPointPos, Quaternion cameraInitPointRotation)
        {
            GameObject camera = await _gameFactory.CreateCamera(cameraInitPointPos);
            camera.transform.rotation = cameraInitPointRotation;
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