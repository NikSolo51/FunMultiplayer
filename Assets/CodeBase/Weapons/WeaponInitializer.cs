using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Weapons.Reload;
using CodeBase.WeaponsInventory;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Weapons
{
    public class WeaponInitializer : MonoBehaviour
    {
        [Required] [SerializeField] private PhotonView _photonView;
        [Required] [SerializeField] private Transform _weaponPosition;
        [SerializeField] private UIIndicator uiIndicator;
        private IPlayerWeaponsInventory _playerWeaponsInventory;
        private IGameFactory _gameFactory;
        private PlayerWeapon playerWeapon;

        [Inject]
        public void Constructor(IPlayerWeaponsInventory playerWeaponsInventory, IGameFactory gameFactory)
        {
            _playerWeaponsInventory = playerWeaponsInventory;
            _gameFactory = gameFactory;
            int weaponId = PhotonNetwork.AllocateViewID(false);
            _photonView.RPC("InitializeWeapon", RpcTarget.All, weaponId);
        }

        [PunRPC]
        private async void InitializeWeapon(int weaponId)
        {
            GameObject weapon = await _gameFactory.CreateWeapon(_playerWeaponsInventory.WeaponType,
                _weaponPosition);
            
            if (_photonView.IsMine)
            {
                playerWeapon = weapon.GetComponent<PlayerWeapon>();
                playerWeapon.OnReloadPercent += uiIndicator.AnimateIndicator;

                PhotonView weaponView = playerWeapon.GetComponent<PhotonView>();
                weaponView.ViewID = weaponId;
                _playerWeaponsInventory.PlayerWeapon = playerWeapon;
            }
        }

        private void OnDestroy()
        {
            if (playerWeapon)
                playerWeapon.OnReloadPercent -= uiIndicator.AnimateIndicator;
        }
    }
}