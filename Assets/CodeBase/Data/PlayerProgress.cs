using System;
using CodeBase.Weapons;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public string _nickName;
        public WeaponType _currentWeapon;
        public PlayerProgress(string initialLevel)
        {
            
        }
    }
}