using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BreadFlip
{
    public class SetLangToggle : MonoBehaviour
    {
        [SerializeField] private Toggle _rusToggle;
        [SerializeField] private Toggle _engToggle;

        private void OnEnable() 
        {
            if (LocalizationManager.Language.Equals("Russian"))
            {
                _rusToggle.isOn = true;
            }
            else
            {
                 _engToggle.isOn = true;
            }
        }
        
    }
}
