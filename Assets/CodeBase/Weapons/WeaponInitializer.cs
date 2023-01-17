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
        [SerializeField] private PhotonView _photonView;
        [HideInInspector] public UIIndicator _reloadIndicator;
        [Required] [SerializeField] private Transform _weaponPosition;
        private IPlayerWeaponsInventory _playerWeaponsInventory;
        private IGameFactory _gameFactory;
        private PlayerWeapon playerWeapon;
        private bool already;
        [Inject]
        public void Constructor(IPlayerWeaponsInventory playerWeaponsInventory, IGameFactory gameFactory)
        {
            _playerWeaponsInventory = playerWeaponsInventory;
            _gameFactory = gameFactory;
        }

        public void Setup()
        {
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
                PhotonView weaponView = playerWeapon.GetComponent<PhotonView>();
                weaponView.ViewID = weaponId;


                playerWeapon.OnReloadPercent += _reloadIndicator.AnimateIndicator;
                _playerWeaponsInventory.PlayerWeapon = playerWeapon;
                already = true;
            }
        }

        public PlayerWeapon GetPlayerWeapon()
        {
            return playerWeapon;
        }

        private void OnDestroy()
        {
            if (playerWeapon)
                playerWeapon.OnReloadPercent -= _reloadIndicator.AnimateIndicator;
        }
    }
}