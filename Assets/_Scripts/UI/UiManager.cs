using System;
using System.Collections;
using BreadFlip.Movement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BreadFlip.UI
{
    public class UiManager : MonoBehaviour
    {
        public static event Action SurvivedAfterFail;

        [Header("Main UI Elements")]
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _gameUi;
        [SerializeField] private GameObject _actionUI;
        [SerializeField] private GameObject losePanel;

        [SerializeField] private Timer _timer;

        [SerializeField] private float _failUIDelay = 0.5f;
        
        [SerializeField] public ToastZoneController zoneController;
        [SerializeField] private JumpController _jumpController;

        public static bool pauseButtonPressed;

        // flags
        private bool onFailedWasInvoked;

        private void Start()
        {
            SetTimeScale(true);
            zoneController.OnCollidedBadThing += OnFailed;
            onFailedWasInvoked = false;
            pauseButtonPressed = false;
            
            if (PlayerPrefs.GetInt("gameNeedsToRestart") == 1)
            {
                RestartGame();
                PlayerPrefs.DeleteKey("gameNeedsToRestart");
            }
        }

        public void PressPauseButton()
        {
            pauseButtonPressed = true;
        }

        public void UnpressPauseButton()
        {
            pauseButtonPressed = false;
        }


        public void SetTimeScale(bool timeShouldGo)
        {
            if (timeShouldGo)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0f;
            }
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnFailed(Vector3 velocity) => StartCoroutine(Fail(velocity));

        public IEnumerator Fail(Vector3 vel)
        {
            if (onFailedWasInvoked) yield break;
            onFailedWasInvoked = true;
            yield return new WaitForSeconds(_failUIDelay);
            
            if (/* Vector3.Dot(Vector3.up, vel) < 0 && */ vel.y > -0.1f)
            {
                onFailedWasInvoked = false;
                SurvivedAfterFail?.Invoke();
                yield break;
            }

            _actionUI.SetActive(false);
            losePanel.SetActive(true);
            SetTimeScale(false);
        }

        public bool Get_onFailedWasInvoked()
        {
            return onFailedWasInvoked;
        }

        public void ReplayGame()
        {
            PlayerPrefs.SetInt("gameNeedsToRestart", 1);
            ReloadScene();
        }

        private void RestartGame()
        {
            // переключаем экраны
            _mainMenu.SetActive(false);
            _gameUi.SetActive(true);

            zoneController.startedInToaster = true;

            // запускаем таймер
            _timer.transform.gameObject.SetActive(true);
            _timer.StartTimerManually();
        }
    }
}