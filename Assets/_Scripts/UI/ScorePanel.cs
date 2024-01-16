using TMPro;
using UnityEngine;

namespace BreadFlip.UI
{
    public class ScorePanel : MonoBehaviour
    {
        private int _score;

        [SerializeField] private UiManager uiManager;
        [SerializeField] private TMP_Text loseScore;
        // [SerializeField] private TMP_Text recordScore;

        private TMP_Text tmp;

        private void Start()
        {
            _score = 0;

            tmp = transform.GetComponentInChildren<TMP_Text>();
            
            uiManager.zoneController.OnCollidedToaster += UpdateTextOnScorePanel;
            Timer.TimeOvered += PrintTextOnLoseScreen;

            uiManager.zoneController.OnCollidedBadThing += PrintTextOnLoseScreen;
        }

        public int GetScore()
        {
            return _score;
        }

        private void UpdateTextOnScorePanel()
        {
            if (!uiManager.zoneController.startedInToaster) _score++;

            tmp.text = _score.ToString();
        }

        private void PrintTextOnLoseScreen()
        {
            DBInterface.UpdateRecord(_score);
            // recordScore.text = PlayerPrefs.GetInt("PlayerRecord", 0).ToString();

            //if (PlayerPrefs.HasKey("record"))
            //{
            //    if (_score > PlayerPrefs.GetInt("record"))
            //    {
            //        PlayerPrefs.SetInt("record", _score);
            //        
            //        recordScore.text = _score.ToString();
            //    }
            //    else
            //    {
            //        recordScore.text = PlayerPrefs.GetInt("record").ToString();
            //    }
            //}
            //else
            //{
            //    PlayerPrefs.SetInt("record", _score);
            //    recordScore.text = _score.ToString();
            //}

            loseScore.text = _score.ToString();
        }
    }
}
