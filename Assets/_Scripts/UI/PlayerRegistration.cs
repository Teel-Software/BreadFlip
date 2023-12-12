using TMPro;
using UnityEngine;

namespace BreadFlip
{
    public class PlayerRegistration : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private GameObject _panel;

        private void Start() 
        {
            if (PlayerPrefs.HasKey("DisableRegistartion"))
            {
                // PlayerPrefs.DeleteKey("DisableRegistartion");
                _panel.SetActive(false);
            }
        }

        public void RegPlayer()
        {
            DBInterface.RegisterPlayer(_inputField.text);
            PlayerPrefs.SetInt("DisableRegistartion", 1);
            _panel.SetActive(false);
        }
    }
}
