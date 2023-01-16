using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.Logic
{
    public class RandomEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent[] _events;

        public void PlayRandomEvent()
        {
            _events[Random.Range(0,_events.Length)]?.Invoke();
        }
    }
}