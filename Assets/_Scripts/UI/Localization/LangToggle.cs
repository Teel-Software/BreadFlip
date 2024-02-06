using UnityEngine;
using UnityEngine.UI;

namespace BreadFlip
{
    public class LangToggle : MonoBehaviour
    {
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Toggle buttonToggle;
        [SerializeField] private TitleIconLang titleIconLang;

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

            titleIconLang.ChangeIcon();


            // if (LocalizationManager.Language == "Russian")
            // {
            //     _titleIcon.sprite = RUS_Title;
            // }
            // else
            // {
            //     _titleIcon.sprite = ENG_Title;
            // }
        }
    }
}
