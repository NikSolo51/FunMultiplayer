using System.Threading.Tasks;
using CodeBase.Infrastructure.Network;
using CodeBase.Services.Audio;
using CodeBase.Services.StaticData;
using CodeBase.SoundManager;
using CodeBase.Weapons;
using Photon.Realtime;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory
    {
        GameObject CreateHero(Vector3 at);
        Task<GameObject> CreateHud();
        Task<GameObject> CreateWeaponUI(WeaponStaticData weaponStaticData,Transform parent);
        Task<GameObject> CreateCamera();
        Task<GameObject> CreateCamera(Vector3 cameraInitPointPos);
        Task<ISoundService> CreateSoundManager(SoundManagerData soundManagerData);
        Task<GameObject> CreateUpdateManager();

        Task<GameObject> CreateWeapon(WeaponType weaponType, Transform parent);
        void CreateRoomButton(RoomInfo roomInfo,NetworkLauncher networkLauncher,Transform parent);
        Task WarmUp();
        void CleanUp();
        void CreatePlayerRoomButton(Player playerInfo, NetworkLauncher networkLauncher, Transform parent);
        Task<GameObject> CreatePlayerUI();
        GameObject CreateGameObject(string key,Vector3 pos,Quaternion rotation);
        GameObject CreateBullet(BulletType bulletType,Vector3 pos,Quaternion rotation);
    }
}