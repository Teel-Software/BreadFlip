﻿using System.Collections;
using BreadFlip.Movement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BreadFlip.UI
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _gameUi;
        [SerializeField] private GameObject _actionUI;
        [SerializeField] private GameObject losePanel;

        [SerializeField] private float _failUIDelay = 0.5f;
        
        [SerializeField] public ToastZoneController zoneController;

        private bool onFailedWasInvoked;

        private void Start()
        {
            SetTimeScale(true);
            zoneController.OnCollidedBadThing += OnFailed;
            onFailedWasInvoked = false;
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
            if (!onFailedWasInvoked)
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
    }
}