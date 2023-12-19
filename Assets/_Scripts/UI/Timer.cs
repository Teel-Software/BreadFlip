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

        public bool TimerEnabled { get; set;}

        public static event Action TimeOvered;
        public static event Action<float> TimerTicked;

        private void OnEnable()
        {
            TimerEnabled = true;
            zoneController.OnCollidedToaster += StartTimer;
            zoneController.OnColliderExit += DisableComponent;
            currentTime = 0;
            StartCoroutine(WaitUntillFail());
        }

        private void OnDestroy()
        {
            zoneController.OnCollidedToaster -= StartTimer;
            zoneController.OnColliderExit -= DisableComponent;
        }

        // public void TimerInit() {
        //     IfGameStarted = true;
        //     gameObject.SetActive(true);
        // }

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
                // StartCoroutine(uiManager.Fail(Vector3.zero));
                TimeOvered?.Invoke();
            }
        }

        public void StartTimer()
        {
            // currentTime = 0;
            // StartCoroutine(WaitUntillFail());
            if (TimerEnabled) gameObject.SetActive(true);
        }
    }
}
