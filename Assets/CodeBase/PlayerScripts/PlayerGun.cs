using CodeBase.Services.Input;
using CodeBase.WeaponsInventory;
using UnityEngine;
using Zenject;

namespace CodeBase.PlayerScripts
{
    public class PlayerGun : MonoBehaviour
    {
        public Transform _weaponInitPoint;
        private IInputService _inputService;
        private IPlayerWeaponsInventory _playerWeaponsInventory;

        [Inject]
        public void Construct(IInputService inputService, IPlayerWeaponsInventory heroWeaponsInventory)
        {
            _inputService = inputService;
            _playerWeaponsInventory = heroWeaponsInventory;
        }

        private void Update()
        {
            if (_inputService != null)
            {
                if (_inputService.IsClickButtonPress())
                {
                    Shoot();
                }
            }
        }

        public void Shoot()
        {
            if (_playerWeaponsInventory.PlayerWeapon != null)
            {
                _playerWeaponsInventory.PlayerWeapon.Shoot();
            }
        }
    }
}