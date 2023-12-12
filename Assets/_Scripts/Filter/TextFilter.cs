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
        [SerializeField] Button button;
        public void CheckText(string str)
        {
            button.enabled = !BadWordManager.Contains(str);
        }
    }
}
