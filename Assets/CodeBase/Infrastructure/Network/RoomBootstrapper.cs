using System;
using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Infrastructure.Factory;
using CodeBase.PlayerScripts;
using CodeBase.Services.StaticData;
using CodeBase.Weapons;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Infrastructure.Network
{
    public class RoomBootstrapper : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform _cameraSpawnPoint;
        [Required] [SerializeField] private DiContainerResolver _diContainerResolver;
        private const string LevelName = "Level1";
        private IGameFactory _gameFactory;
        private IStaticDataService _staticDataService;
        private FollowCamera _followCamera;

        [Inject]
        public void Construct(IGameFactory gameFactory, IStaticDataService staticDataService)
        {
            _gameFactory = gameFactory;
            _staticDataService = staticDataService;
        }

        public override void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public override void OnDisable()
        {
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == LevelName)
            {
                PhotonView photonView = this.GetComponent<PhotonView>();
                int heroId = PhotonNetwork.AllocateViewID(false);
                int cameraId = PhotonNetwork.AllocateViewID(false);
                
                photonView.RPC("Initialize", RpcTarget.AllBuffered, heroId, cameraId);
            }
        }

        [PunRPC]
        private async Task<GameObject> Initialize(int heroId, int cameraId)
        {
            LevelStaticData levelData = _staticDataService.ForLevel(LevelName);
            Vector3 spawnPos = PhotonNetwork.IsMasterClient
                ? levelData.InitialHostPosition
                : levelData.InitialOthePlayerPosition;
            GameObject newPlayer = await _gameFactory.CreateHero(spawnPos);

            PhotonView nView = newPlayer.GetComponentInChildren<PhotonView>();
            nView.ViewID = heroId;

            HeroMove heroMove = newPlayer.GetComponent<HeroMove>();
            _diContainerResolver._container.Inject(heroMove);

            PlayerGun playerGun = newPlayer.GetComponent<PlayerGun>();
            _diContainerResolver._container.Inject(playerGun);

            WeaponInitializer weaponInitializer = newPlayer.GetComponent<WeaponInitializer>();
            _diContainerResolver._container.Inject(weaponInitializer);

            if (nView.IsMine)
            {
                GameObject _cameraGO = await _gameFactory.CreateCamera(_cameraSpawnPoint);
                _cameraGO.transform.rotation = _cameraSpawnPoint.transform.rotation;
                Camera _camera = _cameraGO.GetComponent<Camera>();

                PhotonView photonViewCamera = _camera.GetComponent<PhotonView>();
                photonViewCamera.ViewID = cameraId;

                _followCamera = _camera.GetComponent<FollowCamera>();
                _diContainerResolver._container.Inject(_followCamera);
                _followCamera.Setup(newPlayer);
                heroMove.Setup(_camera);
            }

            return newPlayer;
        }
    }
}