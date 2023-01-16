using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Weapons.Reload
{
    public class UIIndicator : MonoBehaviour
    {
        [SerializeField] private Image _indicator;

        public void AnimateIndicator(float percent)
        {
            _indicator.fillAmount = percent;
        }
    }
}