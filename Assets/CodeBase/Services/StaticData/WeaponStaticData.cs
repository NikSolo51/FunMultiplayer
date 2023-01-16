using CodeBase.Weapons;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services.StaticData
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "StaticData/Weapon")]
    public class WeaponStaticData : ScriptableObject
    {
        public WeaponType _weaponType;
        public AssetReferenceGameObject WeaponPrefabReference;
        public AssetReferenceGameObject BulletPrefabReference;
        public int MagazineCount;
        public float Damage;
        public float ShootDelay = 0.2f;
        public float ReloadDelay = 1;
    }
}