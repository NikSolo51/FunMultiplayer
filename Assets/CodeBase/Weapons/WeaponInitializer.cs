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
        private int weaponId;

        [Inject]
        public void Constructor(IPlayerWeaponsInventory playerWeaponsInventory, IGameFactory gameFactory)
        {
            _playerWeaponsInventory = playerWeaponsInventory;
            _gameFactory = gameFactory;
            InitializeWeapon();
        }


        private async void InitializeWeapon()
        {
            GameObject weapon = await _gameFactory.CreateWeapon(_playerWeaponsInventory.WeaponType,
                _weaponPosition);

            weapon.transform.position = _weaponPosition.position;
            weapon.transform.SetParent(_weaponPosition);
            playerWeapon = weapon.GetComponent<PlayerWeapon>();
            playerWeapon.Construct(_gameFactory);
            
            //playerWeapon.OnReloadPercent += _reloadIndicator.AnimateIndicator;
            _playerWeaponsInventory.PlayerWeapon = playerWeapon;
        }

        private void OnDestroy()
        {
            if (playerWeapon && _reloadIndicator)
                playerWeapon.OnReloadPercent -= _reloadIndicator.AnimateIndicator;
        }
    }
}