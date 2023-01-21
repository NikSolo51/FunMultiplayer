using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Logic.Inidcators
{
    public abstract class UIIndicator : MonoBehaviour
    {
        [SerializeField]protected Image _indicator;
        public abstract void AnimateIndicator(float percent);
    }
}