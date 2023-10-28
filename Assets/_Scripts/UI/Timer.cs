using System;
using BreadFlip.Movement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BreadFlip.UI
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private ToastZoneController zoneController;
        [SerializeField] private UiManager uiManager;

        private static int wholeTime = 6;
        private static int currentTime = 0;

        public bool ifGameStarted = false;

        public static event Action TimeOvered;

        private void OnEnable()
        {
            wholeTime = 6;
            currentTime = 0;
            zoneController.OnCollidedToaster += StartTimer;
            zoneController.OnColliderExit += DisableComponent;
            StartCoroutine(WaitUntillFail());
        }

        private void OnDestroy()
        {
            zoneController.OnCollidedToaster -= StartTimer;
            zoneController.OnColliderExit -= DisableComponent;
        }

        private void DisableComponent()
        {
            gameObject.SetActive(false);
            StopCoroutine(WaitUntillFail());
        }

        private IEnumerator WaitUntillFail()
        {
            for (int i = 0; i < wholeTime; i++)
            {
                _slider.value = wholeTime - currentTime;
                currentTime += 1;
                
                yield return new WaitForSeconds(1f);
            }

            if (currentTime == 6)
            {
                StartCoroutine(uiManager.Fail());
                zoneController.PlayDeadSmoke();
            }
        }

        private void StartTimer()
        {
            if (ifGameStarted) gameObject.SetActive(true);
        }

        public void StartGame(bool start)
        {
            ifGameStarted = start;
        }
    }
}
