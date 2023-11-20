using TMPro;
using UnityEngine;

namespace BreadFlip.UI
{
    public class ChangeTextColor : MonoBehaviour
    {
        public Color defaultColor;
        public Color pressedColor;

        [SerializeField] private TMP_Text buttonText;

        public void ChangeColorOnPressed()
        {
            buttonText.color = pressedColor;
        }

        public void ChangeColorToDefault()
        {
            buttonText.color = defaultColor;
        }
    }
}
