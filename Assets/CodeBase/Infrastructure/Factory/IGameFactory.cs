using System.Threading.Tasks;
using CodeBase.Infrastructure.Network;
using CodeBase.Services.Audio;
using CodeBase.Services.Audio.SoundManager;
using CodeBase.Weapons;
using Photon.Realtime;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory
    {
        Task<GameObject> CreateHero(Vector3 at);
        Task<GameObject> CreateHud();
        Task<GameObject> CreateCamera();
        Task<GameObject> CreateCamera(Transform parent);
        Task<ISoundService> CreateSoundManager(SoundManagerData soundManagerData);
        Task<GameObject> CreateUpdateManager();

        Task<GameObject> CreateWeapon(WeaponType weaponType, Transform parent);
        void CreateRoomButton(RoomInfo roomInfo,NetworkLauncher networkLauncher,Transform parent);
        Task WarmUp();
        void CleanUp();
        void CreatePlayerRoomButton(Player playerInfo, NetworkLauncher networkLauncher, Transform parent);
        Task<GameObject> CreatePlayerUI();
    }
}