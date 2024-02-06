using UnityEngine;
using UnityEngine.UI;

namespace BreadFlip
{
    public class TitleIconLang : MonoBehaviour
    {
        public Sprite ENG_Title;
        public Sprite RUS_Title;

        private Image _titleIcon;

        private void Start() {
            _titleIcon = GetComponent<Image>();
            ChangeIcon();
        }

        public void ChangeIcon()
        {
            if (LocalizationManager.Language == "Russian")
            {
                _titleIcon.sprite = RUS_Title;
            }
            else
            {
                _titleIcon.sprite = ENG_Title;
            }
        }
    }
}
