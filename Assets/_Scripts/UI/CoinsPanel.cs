using System;
using System.Collections;
using System.Collections.Generic;
using BreadFlip.UI;
using TMPro;
using UnityEngine;

namespace BreadFlip
{
    public class CoinsPanel : MonoBehaviour
    {
        private int _coins;

        [SerializeField] private UiManager uiManager;

        public int CollectedCoins
        {
            get {return _coins;}
        }
        
        private TMP_Text tmp;

        private void Start() {
            _coins = 0;

            tmp = transform.GetComponentInChildren<TMP_Text>();

            uiManager.zoneController.OnCollidedCoinAction += UpdateCoinScreen;
        }

        private void UpdateCoinScreen()
        {
            _coins++;
            tmp.text = _coins.ToString();
        }
    }
}
