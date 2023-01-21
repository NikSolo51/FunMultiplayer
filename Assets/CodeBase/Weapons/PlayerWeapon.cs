using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.StaticData;
using UnityEngine;

namespace CodeBase.Weapons
{
    public abstract class PlayerWeapon : MonoBehaviour, IWeapon
    {
        public abstract void Construct(IGameFactory gameFactory);
       
        public bool NotEmpty { get; set; }
        public int MagazineCount { get; set; }
        public float Damage { get; set; }
        public float ShootDelay { get; set; }
        public float ReloadDelay { get; set; }
        public WeaponType WeaponType { get; set; }
        public BulletType BulletType { get; set; }

        public abstract void Shoot();
        public abstract event Action<float> OnReloadPercent;
        public abstract event Action<int> OnAmmoCount;
    }
}