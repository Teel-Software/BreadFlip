using UnityEngine;
using UnityEngine.UI;

namespace BreadFlip
{
    public class LangToggle : MonoBehaviour
    {
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Toggle buttonToggle;
        [SerializeField] private Image _titleIcon;
        [SerializeField] private Sprite ENG_Title;
        [SerializeField] private Sprite RUS_Title;

        public void ActionOnChange()
        {
            if (buttonToggle.isOn)
            {
                GetComponent<Image>().sprite = buttonToggle.spriteState.selectedSprite;
            }
            else
            {
                GetComponent<Image>().sprite = defaultSprite;
            }

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
