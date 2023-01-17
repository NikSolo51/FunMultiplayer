using System;
using Photon.Pun;

namespace CodeBase.Weapons.Reload
{
    public class ReloadUIIndicator : UIIndicator
    {
        public override void AnimateIndicator(float percent)
        {
            _indicator.fillAmount = percent;
        }
    }
}