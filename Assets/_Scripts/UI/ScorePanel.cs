using TMPro;
using UnityEngine;

namespace BreadFlip.UI
{
    public class ScorePanel : MonoBehaviour
    {
        private int _score;

        [SerializeField] private UiManager uiManager;
        [SerializeField] private TMP_Text loseScore;

        private TMP_Text tmp;


        private void Start()
        {
            _score = 0;

            tmp = transform.GetComponentInChildren<TMP_Text>();
            
            uiManager.zoneController.OnCollidedToaster += UpdateTextOnScorePanel;
            
            uiManager.zoneController.OnCollidedBadThing += PrintTextOnLoseScreen;
        }

        public int GetScore()
        {
            return _score;
        }

        private void UpdateTextOnScorePanel()
        {
            _score++;
            tmp.text = _score.ToString();
        }

        private void PrintTextOnLoseScreen()
        {
            loseScore.text = _score.ToString();
        }
    }
}
