using System;
using Photon.Pun;
using UnityEngine;

namespace CodeBase.Weapons
{
    public abstract class PlayerWeapon : MonoBehaviour, IWeapon
    {

        public bool NotEmpty { get; set; }
        public int MagazineCount { get; set; }
        public float Damage { get; set; }
        public float ShootDelay { get; set; }
        public float ReloadDelay { get; set; }

        public WeaponType WeaponType { get; set; }
    
        public abstract void Shoot();
        

        public abstract event Action<float> OnReloadPercent;

    }
}