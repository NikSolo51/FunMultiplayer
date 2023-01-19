using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Network;
using CodeBase.Services.Audio;
using CodeBase.Services.Audio.SoundManager;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI;
using CodeBase.Weapons;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory, IPunPrefabPool
    {
        private IAssets _asset;
        private IStaticDataService _staticData;
        private ISaveLoadService _saveLoadService;
        private DiContainer _container;

        public GameFactory(IAssets assets,
            IStaticDataService staticData,
            ISaveLoadService saveLoadService,
            DiContainer container)
        {
            _asset = assets;
            _staticData = staticData;
            _saveLoadService = saveLoadService;
            _container = container;
            PhotonNetwork.PrefabPool = this;
        }

        public async Task WarmUp()
        {
            await _asset.Load<GameObject>(AssetsAdress.Hero);
            await _asset.Load<GameObject>(AssetsAdress.RoomButtonUI);
            await _asset.Load<GameObject>(AssetsAdress.PlayerButtonUI);
            await _asset.Load<GameObject>(AssetsAdress.UpdateManager);
            await _asset.Load<GameObject>(AssetsAdress.PlayerUI);
            await _asset.Load<GameObject>(AssetsAdress.WeaponUI);
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetsAdress.Hud);
            return hud;
        }

        public async Task<GameObject> CreateWeaponUI(WeaponStaticData weaponStaticData, Transform parent)
        {
            GameObject weaponUIGO = await InstantiateRegisteredAsync(AssetsAdress.WeaponUI, parent);
            WeaponUI weaponUI = weaponUIGO.GetComponent<WeaponUI>();
            _container.Inject(weaponUI);
            weaponUI.Initialize(weaponStaticData);
            return weaponUIGO;
        }

        public async Task<GameObject> CreateWeapon(WeaponType weaponType, Transform parent)
        {
            WeaponStaticData weaponStaticData = _staticData.ForWeapon(weaponType);

            //GameObject weaponReference = await _asset.Load<GameObject>(weaponStaticData.WeaponPrefabReference);
            GameObject weaponGO =
                PhotonNetwork.Instantiate(weaponType.ToString(), parent.position, Quaternion.identity);
            //GameObject weaponGO = InstantiateRegistered(weaponReference, parent);

            PlayerWeapon playerWeapon = weaponGO.GetComponentInChildren<PlayerWeapon>();

            playerWeapon.BulletType = weaponStaticData._bulletType;
            playerWeapon.Damage = weaponStaticData.Damage;
            playerWeapon.MagazineCount = weaponStaticData.MagazineCount;
            playerWeapon.WeaponType = weaponStaticData._weaponType;
            playerWeapon.ShootDelay = weaponStaticData.ShootDelay;
            playerWeapon.ReloadDelay = weaponStaticData.ReloadDelay;

            return weaponGO;
        }

        public async void CreateRoomButton(RoomInfo roomInfo, NetworkLauncher networkLauncher, Transform parent)
        {
            GameObject roomButton = await InstantiateRegisteredAsync(AssetsAdress.RoomButtonUI, parent);
            roomButton.transform.SetParent(parent);
            RoomListItem roomListItem = roomButton.GetComponent<RoomListItem>();
            roomListItem.Constructor(roomInfo, networkLauncher);
        }

        public async void CreatePlayerRoomButton(Player playerInfo, NetworkLauncher networkLauncher, Transform parent)
        {
            GameObject roomButton = await InstantiateRegisteredAsync(AssetsAdress.PlayerButtonUI, parent);
            roomButton.transform.SetParent(parent);
            PlayerListItem playerListItem = roomButton.GetComponent<PlayerListItem>();
            playerListItem.Constructor(playerInfo);
        }

        public GameObject CreateGameObject(string key, Vector3 pos, Quaternion rotation)
        {
            return PhotonNetwork.Instantiate(key, pos, rotation);
        }

        public GameObject CreateBullet(BulletType bulletType, Vector3 pos, Quaternion rotation)
        {
            return PhotonNetwork.Instantiate(bulletType.ToString(), pos, rotation);
        }

        public async Task<ISoundService> CreateSoundManager(SoundManagerData soundManagerData)
        {
            SoundManagerStaticData soundManagerManagerStaticData =
                _staticData.ForSoundManager(soundManagerData._soundManagerType);

            if (soundManagerData._soundManagerType == SoundManagerType.Nothing)
                Debug.Log("SoundManager Type is Nothing");

            GameObject soundManagerPrefab = await _asset.Load<GameObject>(soundManagerManagerStaticData.SoundManager);

            GameObject soundManagerObject = InstantiateRegistered(soundManagerPrefab);
            SoundManagerAbstract soundManagerAbstract = soundManagerObject.GetComponent<SoundManagerAbstract>();

            soundManagerAbstract.sounds = soundManagerData._sounds;
            soundManagerAbstract.clips = soundManagerData._clips;

            return soundManagerAbstract;
        }


        public GameObject CreateHero(Vector3 at)
        {
            //GameObject HeroGameObject = await InstantiateRegisteredAsync(AssetsAdress.Hero, at);
            GameObject HeroGameObject = PhotonNetwork.Instantiate(AssetsAdress.Hero, at, Quaternion.identity);
            return HeroGameObject;
        }

        public async Task<GameObject> CreatePlayerUI()
        {
            GameObject playerUI = await InstantiateRegisteredAsync(AssetsAdress.PlayerUI);
            return playerUI;
        }

        public async Task<GameObject> CreateUpdateManager()
        {
            GameObject updateManager = await InstantiateRegisteredAsync(AssetsAdress.UpdateManager);
            return updateManager;
        }

        public async Task<GameObject> CreateCamera()
        {
            GameObject cameraGameObject = await InstantiateAsync(AssetsAdress.Camera);

            return cameraGameObject;
        }

        public async Task<GameObject> CreateCamera(Vector3 cameraInitPointPos)
        {
            GameObject cameraGameObject =
                await InstantiateAsync(AssetsAdress.Camera, cameraInitPointPos);
            return cameraGameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }


        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath, at: at);

            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Transform parent)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath, parent);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }


        private async Task<GameObject> InstantiateAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath, at: at);
            return gameObject;
        }

        private async Task<GameObject> InstantiateAsync(string prefabPath)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at, Transform parent)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity, parent);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at, Quaternion rotation)
        {
            GameObject gameObject = GameObject.Instantiate(prefab, at, rotation);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab, Transform parent)
        {
            GameObject gameObject = Object.Instantiate(prefab, parent);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            ISavedProgressReader[] readers = gameObject.GetComponentsInChildren<ISavedProgressReader>();
            for (var index = 0; index < readers.Length; index++)
            {
                ISavedProgressReader progressReader = readers[index];
                _saveLoadService.Register(progressReader);
            }
        }

        public void CleanUp()
        {
            _saveLoadService.CleanUp();
            _asset.CleanUp();
        }

        public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = _asset.LoadSynchronously<GameObject>(prefabId);

            if (rotation == Quaternion.identity) rotation = prefab.transform.rotation;

            GameObject gameObject = InstantiateRegistered(prefab, position, rotation);
            return gameObject;
        }

        public void Destroy(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }
    }
}