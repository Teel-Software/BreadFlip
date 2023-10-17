using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BreadFlip.UI
{
    public class ToggleSwitcher : MonoBehaviour
    {
        public bool isOn { get; set; }

        [SerializeField] GameObject on;
        [SerializeField] GameObject off;

        protected Toggle toggle;

        protected void OnEnable()
        {
            StartAction();
        }

        public virtual void StartAction()
        {
            toggle = GetComponent<Toggle>();
            isOn = toggle.isOn;
            MoveIndicator(isOn);
        }

        protected void MoveIndicator(bool value)
        {
            if (value)
            {
                on.SetActive(true);
                off.SetActive(false);
            }

            else
            {
                on.SetActive(false);
                off.SetActive(true);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isOn = toggle.isOn;
            MoveIndicator(!isOn);
        }
    }
}
