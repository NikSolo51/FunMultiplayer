using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.SaveLoad;
using CodeBase.Weapons;
using UnityEngine;

namespace CodeBase.WeaponsInventory
{
    public class PlayerWeaponsInventory : IPlayerWeaponsInventory
    {
        public WeaponType WeaponType
        {
            get => _weaponType;
            set => _weaponType = value;
        }

        public PlayerWeapon PlayerWeapon { get; set; }
        
        private List<WeaponType> _weapons = new List<WeaponType>();
        private WeaponType _currentWeaponType = WeaponType.Shotgun;
        private GameObject _currentWeaponGameObject;
        private PlayerWeapon _currentPlayerWeapon;
        private WeaponType _weaponType = WeaponType.Shotgun;


        public void AddWeapon(WeaponType weaponType)
        {
            if (!_weapons.Contains(weaponType))
                _weapons.Add(weaponType);
        }


        public void CleanUp()
        {
            _weapons.Clear();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _currentWeaponType = progress._currentWeapon;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress._currentWeapon = _weaponType;
        }
    }
}