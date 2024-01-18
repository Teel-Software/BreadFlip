using System;
using UnityEngine;
using UnityEngine.UI;

namespace BreadFlip.UI
{
    public class MarketCell : MonoBehaviour
    {
        public Toggle buttonToggle;
        public static Action<Skins> SkinSelected;

        public Skins skin;

        [SerializeField ] private Sprite defaultImage;

        private void Awake() {
            buttonToggle.group = transform.parent.GetComponent<ToggleGroup>();
        }

        public void SelectSkin()
        {
            if (buttonToggle.isOn)
            {
                GetComponent<Image>().sprite = buttonToggle.spriteState.selectedSprite;
                SkinSelected.Invoke(skin);
            }
            else
            {
                GetComponent<Image>().sprite = defaultImage;
            }
        }
    }
}
