using UnityEngine;

namespace CodeBase.Weapons.Reload
{
    public class HealthUIIndicator : UIIndicator
    {
        public override void AnimateIndicator(float percent)
        {
            _indicator.fillAmount = percent;
        }
    }
}