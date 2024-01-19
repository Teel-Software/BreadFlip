using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.BWF;
using Crosstales.BWF.Manager;
using UnityEngine.UI;

namespace BreadFlip
{
    public class TextFilter : MonoBehaviour
    {
        [SerializeField] private GameObject _label;

        [SerializeField] Button button;

        private void OnEnable()
        {
            button.enabled = false;
        }

        public void CheckText(string str)
        {
            var a = str.Trim();
            button.enabled = !BadWordManager.Contains(str) && (a != "");
            _label.SetActive(!button.enabled);
        }
    }
}
