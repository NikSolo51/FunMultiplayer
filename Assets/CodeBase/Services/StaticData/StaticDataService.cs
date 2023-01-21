using System.Collections.Generic;
using System.Linq;
using CodeBase.SoundManager;
using CodeBase.Weapons;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<SoundManagerType, SoundManagerStaticData> _soundManagers;
        private Dictionary<WeaponType, WeaponStaticData> _weapons;

        public void Initialize()
        {
            IList<IResourceLocation> resourceLocations =
                Addressables.LoadResourceLocationsAsync("Level", typeof(LevelStaticData)).WaitForCompletion();
            IList<LevelStaticData> levelStaticData =
                Addressables.LoadAssets<LevelStaticData>(resourceLocations, null).WaitForCompletion();

            _levels = levelStaticData.ToDictionary(x => x.LevelKey, x => x);

            IList<IResourceLocation> resourceLocationsSoundSystem =
                Addressables.LoadResourceLocationsAsync("SoundManager", typeof(SoundManagerStaticData))
                    .WaitForCompletion();
            IList<SoundManagerStaticData> soundSystems =
                Addressables.LoadAssets<SoundManagerStaticData>(resourceLocationsSoundSystem, null)
                    .WaitForCompletion();
            
            _soundManagers = soundSystems.ToDictionary(x => x.SoundManagerType, x => x);

            IList<IResourceLocation> resourceLocationsWeapons =
                Addressables.LoadResourceLocationsAsync("WeaponData", typeof(WeaponStaticData))
                    .WaitForCompletion();
            IList<WeaponStaticData> weapons =
                Addressables.LoadAssets<WeaponStaticData>(resourceLocationsWeapons, null)
                    .WaitForCompletion();
            
            _weapons = weapons.ToDictionary(x => x._weaponType, x => x);

        }

        public LevelStaticData ForLevel(string sceneKey)
        {
            return _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
                ? staticData
                : null;
        }

        public SoundManagerStaticData ForSoundManager(SoundManagerType soundManagerType)
        {
            return _soundManagers.TryGetValue(soundManagerType, out SoundManagerStaticData staticData)
                ? staticData
                : null;
        }

        public WeaponStaticData ForWeapon(WeaponType weaponType)
        {
            return _weapons.TryGetValue(weaponType, out WeaponStaticData staticData)
                ? staticData
                : null;
        }
        
        public WeaponStaticData[] AllWeapons()
        {
            return _weapons.Values.ToArray();
        }
    }
}