using System;
using CodeBase.Data;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.Weapons;
using CodeBase.WeaponsInventory;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class WeaponUI : MonoBehaviour
    {
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private TMP_Text _textTMP;
        private IPlayerWeaponsInventory _playerWeaponsInventory;

        [Inject]
        public void Constructor(IPlayerWeaponsInventory playerWeaponsInventory)
        {
            _playerWeaponsInventory = playerWeaponsInventory;
        }

        public void Initialize(WeaponStaticData weaponStaticData)
        {
            weaponType = weaponStaticData._weaponType;
            _textTMP.text = weaponType.ToString();
        }

        public void SelectWeapon()
        {
            _playerWeaponsInventory.WeaponType = weaponType;
        }
    }
}