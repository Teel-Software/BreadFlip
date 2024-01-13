using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadFlip
{
    public class SettingsBackButton : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _mainMenu;
        
        public void OpenPausePanel()
        {
            if (_mainMenu.activeSelf)
            {
                _pausePanel.SetActive(false);
            }
            else
            {
                _pausePanel.SetActive(true);
            }
        }
    }
}
