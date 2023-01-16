using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeBase.Logic
{
    public class Toggle : MonoBehaviour
    {
        [SerializeField] private Image buttonImage;
        private bool toggle;
        public UnityEvent ToggleOn;
        public UnityEvent ToggleOff;

        public void Switch()
        {
            toggle = !toggle;
            if (toggle)
            {
                ToggleOn?.Invoke();
            }
            else
            {
                ToggleOff?.Invoke();
            }
        }
    }
}