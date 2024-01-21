using UnityEngine;
using UnityEngine.UI;

namespace BreadFlip
{
    public class LangToggle : MonoBehaviour
    {
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Toggle buttonToggle;

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
        }
    }
}
