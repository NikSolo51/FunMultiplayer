using CodeBase.Services.SaveLoad;
using CodeBase.Weapons;

namespace CodeBase.WeaponsInventory
{
    public interface IPlayerWeaponsInventory : ISavedProgress
    {
        WeaponType WeaponType { get; set; }
        PlayerWeapon PlayerWeapon { get; set; }
        void AddWeapon(WeaponType weaponType);
        void CleanUp();
    }
}