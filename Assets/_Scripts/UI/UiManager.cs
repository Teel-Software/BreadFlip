using System.Collections;
using BreadFlip.Movement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BreadFlip.UI
{
    public class UiManager : MonoBehaviour
    {
        [Header("Main UI Elements")]
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _gameUi;
        [SerializeField] private GameObject _actionUI;
        [SerializeField] private GameObject losePanel;

        [SerializeField] private Timer _timer;

        [SerializeField] private float _failUIDelay = 0.5f;
        
        [SerializeField] public ToastZoneController zoneController;

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

        public void OnFailed()
        {
            if (!onFailedWasInvoked && !Input.GetMouseButtonDown(0))
            {
                _actionUI.SetActive(false);
                StartCoroutine(Fail());
            }
            onFailedWasInvoked = true;
        }

        public IEnumerator Fail()
        {
            yield return new WaitForSeconds(_failUIDelay);
            
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