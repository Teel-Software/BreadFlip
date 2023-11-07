using BreadFlip.Movement;
using BreadFlip.UI;
using System.Collections;
using UnityEngine;

namespace BreadFlip.Entities
{
    public class ToastColorChanger : MonoBehaviour
    {
        [SerializeField] private Renderer _PulpRenderer;
        [SerializeField] private Renderer _CrustRenderer;

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
            _CrustRenderer.material.color = Color.Lerp(startCrustColor, endCrustColor, tick / Timer.wholeTime);
            _PulpRenderer.material.color = Color.Lerp(startPulpColor, endPulpColor, tick / Timer.wholeTime);
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
            _PulpRenderer.material.color = startPulpColor;
            _CrustRenderer.material.color = startCrustColor;
        }

        private void ResetColor()
        {
            StartCoroutine(ResetColorAnimation());
        }
    }
}
