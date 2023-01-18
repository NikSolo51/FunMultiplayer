using CodeBase.Services.StaticData;

namespace CodeBase.Weapons
{
    public interface IWeapon
    {
        public bool NotEmpty { get; set; }
        public int MagazineCount { get; set; }
        public float Damage { get; set; }
        public float ShootDelay { get; set; }
        public WeaponType WeaponType { get; }
        public BulletType BulletType { get; }
        public void Shoot();
    }
}