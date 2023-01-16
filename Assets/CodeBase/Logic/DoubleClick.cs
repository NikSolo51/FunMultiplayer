using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Logic
{
    public class DoubleClick : MonoBehaviour
    {
        private float firstClickTime;
        private float timeBetweenClick = 0.5f;
        private bool isTimeCheckAllowed = true;
        private int clickNum = 0;
        public event Action OnDoubleClick;

        public void StartDetect()
        {
            clickNum += 1;

            if (clickNum == 1 && isTimeCheckAllowed)
            {
                firstClickTime = Time.time;
                StartCoroutine(DetectDoubleClick());
            }
        }

        public IEnumerator DetectDoubleClick()
        {
            isTimeCheckAllowed = false;
            while (Time.time < firstClickTime + timeBetweenClick)
            {
                if (clickNum == 2)
                {
                    OnDoubleClick?.Invoke();
                    break;
                }

                yield return new WaitForEndOfFrame();
            }

            clickNum = 0;
            isTimeCheckAllowed = true;
        }
    }
}