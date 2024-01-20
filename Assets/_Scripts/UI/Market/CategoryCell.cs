using System;
using UnityEngine;
using UnityEngine.UI;

namespace BreadFlip.UI
{
    public class CategoryCell : MonoBehaviour
    {
        public Toggle buttonToggle;

        public static Action<CategoryType> CategorySelected;

        public CategoryType catType;

        [SerializeField] private Sprite defaultImage;

        public void SelectCategory()
        {
            if (buttonToggle.isOn)
            {
                GetComponent<Image>().sprite = buttonToggle.spriteState.selectedSprite;
                CategorySelected.Invoke(catType);
            }
            else
            {
                GetComponent<Image>().sprite = defaultImage;
            }
        }
    }
}
