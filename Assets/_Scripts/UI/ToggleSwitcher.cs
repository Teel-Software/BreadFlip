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

        private Toggle toggle;

        void OnEnable()
        {
            StartAction();
        }

        public virtual void StartAction()
        {
            toggle = GetComponent<Toggle>();
            isOn = toggle.isOn;
            MoveIndicator(isOn);
        }

        private void MoveIndicator(bool value)
        {
            Debug.Log("Я зашёл");
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

        public void RunMoveIndicator()
        {
            isOn = toggle.isOn;
            MoveIndicator(isOn);
        }

        //public void OnPointerDown(PointerEventData eventData)
        //{
        //isOn = toggle.isOn;
        //MoveIndicator(!isOn);
        //}
    }
}
