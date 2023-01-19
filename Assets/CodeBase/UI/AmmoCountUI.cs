using System;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class AmmoCountUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentAmmoText;
        [SerializeField] private TMP_Text _maxAmmoText;

        public void InitializeMaxAmmo(int maxAmmo)
        {
            _maxAmmoText.text = maxAmmo.ToString();
        }

        public void UpdateAmmoText(int count)
        {
            _currentAmmoText.text = count.ToString();
        }
    }
}