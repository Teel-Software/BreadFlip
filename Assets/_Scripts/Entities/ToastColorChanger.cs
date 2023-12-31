using BreadFlip.Movement;
using BreadFlip.UI;
using System.Collections;
using UnityEngine;

namespace BreadFlip.Entities
{
    public class ToastColorChanger : MonoBehaviour
    {
        [SerializeField] private Toast _toast;

        [Header("Crust")]
        [SerializeField] private Color startCrustColor;
        [SerializeField] private Color endCrustColor;

        [Header("Pulp")]
        [SerializeField] private Color startPulpColor;
        [SerializeField] private Color endPulpColor;

        [SerializeField] private ToastZoneController _zoneController;

        [SerializeField] private float resetDuration = 0.5f;

        private void Start()
        {
            Timer.TimerTicked += ChangeColor;
            _zoneController.OnColliderExit += ResetColor;
        }
        private void OnDestroy()
        {
            Timer.TimerTicked -= ChangeColor;
            _zoneController.OnColliderExit -= ResetColor;
        }

        private void ChangeColor(float tick)
        {
            _toast.CrustRenderer.material.color = Color.Lerp(startCrustColor, endCrustColor, tick / Timer.wholeTime);
            _toast.PulpRenderer.material.color = Color.Lerp(startPulpColor, endPulpColor, tick / Timer.wholeTime);
        }

        private IEnumerator ResetColorAnimation()
        {
            var currentTime = resetDuration;
            while (currentTime >= 0f)
            {
                currentTime -= Time.deltaTime;
                ChangeColor(currentTime * Timer.wholeTime / resetDuration);
                yield return null;
            }
            _toast.PulpRenderer.material.color = startPulpColor;
            _toast.CrustRenderer.material.color = startCrustColor;
        }

        private void ResetColor()
        {
            StartCoroutine(ResetColorAnimation());
        }
    }
}
