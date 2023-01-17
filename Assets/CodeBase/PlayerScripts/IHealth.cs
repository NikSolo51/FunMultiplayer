using System;

namespace CodeBase.PlayerScripts
{
    public interface IHealth
    {
        void TakeDamage(float damage);
        public event Action<float> OnHpPercent;
    }
}