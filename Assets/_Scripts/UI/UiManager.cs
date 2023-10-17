using UnityEngine;
using UnityEngine.SceneManagement;

namespace BreadFlip.UI
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _gameUi;
       
        
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
    }
}