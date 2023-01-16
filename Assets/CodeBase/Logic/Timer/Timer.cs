using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.Logic.Timer
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private float _maxTime;
        public UnityEvent OnTimeEnd;

        public void StartTimer()
        {
            
            StartCoroutine(TimerCounter());
        }

        public void StopTimer()
        {
            StopAllCoroutines();
        }

        IEnumerator TimerCounter()
        {
            float elapsedTime = 0;
            while (elapsedTime < _maxTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            OnTimeEnd?.Invoke();
        }
    }
}