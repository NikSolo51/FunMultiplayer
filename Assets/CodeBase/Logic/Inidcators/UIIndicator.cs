using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Weapons.Reload
{
    public abstract class UIIndicator : MonoBehaviour
    {
        [SerializeField]protected Image _indicator;
        public abstract void AnimateIndicator(float percent);
    }
}