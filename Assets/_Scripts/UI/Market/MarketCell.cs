using System;
using UnityEngine;
using UnityEngine.UI;

namespace BreadFlip.UI
{
    public class MarketCell : MonoBehaviour
    {
        [SerializeField] private Toggle _buttonToggle;
        public static Action<Skins> SkinSelected;

        public Skins skin;

        [SerializeField ] private Sprite defaultImage;

        private void OnEnable() {
            _buttonToggle.group = transform.parent.GetComponent<ToggleGroup>();
        }

        public void SelectSkin()
        {
            if (_buttonToggle.isOn)
            {
                GetComponent<Image>().sprite = _buttonToggle.spriteState.selectedSprite;
                SkinSelected.Invoke(skin);
            }
            else
            {
                GetComponent<Image>().sprite = defaultImage;
            }
        }
    }
}
