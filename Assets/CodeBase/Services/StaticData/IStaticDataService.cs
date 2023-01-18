using CodeBase.Services.Audio.SoundManager;
using CodeBase.Weapons;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService 
    {
        void Initialize();
        LevelStaticData ForLevel(string sceneKey);

        SoundManagerStaticData ForSoundManager(SoundManagerType soundManagerType);
        WeaponStaticData ForWeapon(WeaponType weaponType);
        WeaponStaticData[] AllWeapons();
    }
}