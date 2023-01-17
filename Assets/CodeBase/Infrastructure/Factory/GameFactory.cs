using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Network;
using CodeBase.Services.Audio;
using CodeBase.Services.Audio.SoundManager;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.Weapons;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private IAssets _asset;
        private IStaticDataService _staticData;
        private ISaveLoadService _saveLoadService;

        public GameFactory(IAssets assets,
            IStaticDataService staticData,
            ISaveLoadService saveLoadService)
        {
            _asset = assets;
            _staticData = staticData;
            _saveLoadService = saveLoadService;
        }

        public async Task WarmUp()
        {
            await _asset.Load<GameObject>(AssetsAdress.Hero);
            await _asset.Load<GameObject>(AssetsAdress.RoomButtonUI);
            await _asset.Load<GameObject>(AssetsAdress.PlayerButtonUI);
            await _asset.Load<GameObject>(AssetsAdress.UpdateManager);
            await _asset.Load<GameObject>(AssetsAdress.PlayerUI);
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetsAdress.Hud);
            return hud;
        }

        public async Task<GameObject> CreateWeapon(WeaponType weaponType, Transform parent)
        {
            WeaponStaticData weaponStaticData = _staticData.ForWeapon(weaponType);

            GameObject weaponReference = await _asset.Load<GameObject>(weaponStaticData.WeaponPrefabReference);
            GameObject weaponGO = InstantiateRegistered(weaponReference, parent);
            PlayerWeapon playerWeapon = weaponGO.GetComponentInChildren<PlayerWeapon>();
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


        public async Task<GameObject> CreateHero(Vector3 at)
        {
            GameObject HeroGameObject = await InstantiateRegisteredAsync(AssetsAdress.Hero, at);
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

        public async Task<GameObject> CreateCamera(Transform cameraSpawnPoint)
        {
            GameObject cameraGameObject = await InstantiateAsync(AssetsAdress.Camera, cameraSpawnPoint.position);
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
    }
}