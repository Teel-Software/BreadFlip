using System;
using BreadFlip.Movement;
using System.Collections;
using UnityEngine;

namespace BreadFlip.UI
{
    public class Timer : MonoBehaviour
    {
        //[SerializeField] private Slider _slider;
        [SerializeField] private ToastZoneController zoneController;
        [SerializeField] private UiManager uiManager;

        public static int wholeTime => 6;
        private static int currentTime = 0;

        public bool ifGameStarted = false;

        public static event Action TimeOvered;
        public static event Action<float> TimerTicked;

        private void OnEnable()
        {
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

        // чтобы таймер не шёл после уже случившего проигрыша
        private void Update()
        {
            if (uiManager.Get_onFailedWasInvoked())
            {
                StopCoroutine(WaitUntillFail());
            }
        }

        private IEnumerator WaitUntillFail()
        {
            for (int i = 0; i <= wholeTime; i++)
            {
                //_slider.value = wholeTime - currentTime;
                currentTime += 1;
                
                yield return new WaitForSeconds(1f);
                TimerTicked?.Invoke(currentTime);
            }

            if (currentTime >= 6)
            {
                // перенести подписку на событие в менеджер
                StartCoroutine(uiManager.Fail(Vector3.zero));
                TimeOvered?.Invoke();
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

        public void StartTimerManually()
        {
            ifGameStarted = true;
        }
    }
}
