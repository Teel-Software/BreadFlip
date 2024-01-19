using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.BWF;
using Crosstales.BWF.Manager;
using UnityEngine.UI;
using BreadFlip.UI;

namespace BreadFlip
{
    public class TextFilter : MonoBehaviour
    {
        [SerializeField] private GameObject _label;

        [SerializeField] Button button;

        private void OnEnable()
        {
            button.interactable = false;
            button.gameObject.GetComponent<ChangeTextColor>().ChangeColorOnPressed();
        }

        public void CheckText(string str)
        {
            var a = str.Trim();
            // button.interactable = !BadWordManager.Contains(str) && (a != "");
            if (!BadWordManager.Contains(str) && (a != ""))
            {
                button.interactable = true;
                button.gameObject.GetComponent<ChangeTextColor>().ChangeColorToDefault();
            }
            else
            {
                button.interactable = false;
                button.gameObject.GetComponent<ChangeTextColor>().ChangeColorOnPressed();
            }
            _label.SetActive(!button.enabled);
        }
    }
}
