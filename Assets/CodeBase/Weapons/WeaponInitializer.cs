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
    public class WeaponInitializer : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PhotonView _photonView;
        [HideInInspector] public UIIndicator _reloadIndicator;
        [Required] [SerializeField] private Transform _weaponPosition;
        private IPlayerWeaponsInventory _playerWeaponsInventory;
        private IGameFactory _gameFactory;
        private PlayerWeapon playerWeapon;
        private bool already;
        private int weaponId;

        [Inject]
        public void Constructor(IPlayerWeaponsInventory playerWeaponsInventory, IGameFactory gameFactory)
        {
            _playerWeaponsInventory = playerWeaponsInventory;
            _gameFactory = gameFactory;
        }

        public void Setup(int weaponId)
        {
            _photonView.RPC("InitializeWeapon", RpcTarget.All, weaponId);
        }

        [PunRPC]
        private async void InitializeWeapon(int weaponId)
        {
            if (_photonView.IsMine)
            {
                if (already)
                    return;
            }

            GameObject weapon = await _gameFactory.CreateWeapon(_playerWeaponsInventory.WeaponType,
                _weaponPosition);
            playerWeapon = weapon.GetComponent<PlayerWeapon>();
            PhotonView weaponView = playerWeapon.GetComponent<PhotonView>();
            weaponView.ViewID = weaponId;

            if (_photonView.IsMine)
            {
                //playerWeapon.OnReloadPercent += _reloadIndicator.AnimateIndicator;
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
            if (playerWeapon && _reloadIndicator)
                playerWeapon.OnReloadPercent -= _reloadIndicator.AnimateIndicator;
        }
    }
}