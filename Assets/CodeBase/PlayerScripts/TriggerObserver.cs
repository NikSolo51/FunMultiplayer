using System;
using UnityEngine;

namespace CodeBase.PlayerScripts
{
    [RequireComponent(typeof(Collider))]
    public class TriggerObserver : MonoBehaviour
    {
        public event Action TriggerEnter;
        public event Action TriggerExit;
        public event Action<Collider> TriggerEnterCollider;
        public event Action<Collider> TriggerExitCollider;

        private void OnTriggerEnter(Collider other)
        {
            TriggerEnter?.Invoke();
            TriggerEnterCollider?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExit?.Invoke();
            TriggerExitCollider?.Invoke(other);
        }
    }
}