using TMPro;
using UnityEngine;

namespace BreadFlip.UI
{
    public class CoinsPanel : MonoBehaviour
    {
        public int Coins {get; private set;}

        [SerializeField] private UiManager uiManager;
        [SerializeField] private TMP_Text _coinsOutput;
        
        private TMP_Text tmp;

        private void Start() {
            Coins = 0;

            tmp = transform.GetComponentInChildren<TMP_Text>();

            uiManager.zoneController.OnCollidedCoinAction += UpdateCoinScreen;
            Timer.TimeOvered += PrintCoinsOnLoseScreen;

            uiManager.zoneController.OnCollidedBadThing += PrintCoinsOnLoseScreen;
        }

        private void UpdateCoinScreen()
        {
            Coins++;
            tmp.text = Coins.ToString();
        }

        private void PrintCoinsOnLoseScreen()
        {
            if (PlayerPrefs.HasKey("all_coins"))
            {
                var coinsWas = PlayerPrefs.GetInt("all_coins");
                PlayerPrefs.SetInt("all_coins", coinsWas + Coins);
            }
            else
            {
                PlayerPrefs.SetInt("all_coins", Coins);
            }
            _coinsOutput.text = $"+{Coins}";
        
        }
    }
}
