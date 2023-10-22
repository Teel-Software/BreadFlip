using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BreadFlip
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private int wholeTime = 6;
        private int currentTime = 0;

        private void OnEnable()
        { 
            StartCoroutine(newIen());
        }

        private IEnumerator newIen()
        {
            for (int i = 0; i < wholeTime; i++)
            {
                _slider.value = wholeTime - currentTime;
                currentTime += 1;

                Debug.Log($"TIMEEERR: {wholeTime - currentTime}");

                yield return new WaitForSeconds(1f);
            }
                        

            

            

            if (currentTime == 6)
            {
                // вызывается событие на проигрыш
            }
        }
    }
}
