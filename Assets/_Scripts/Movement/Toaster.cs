using System;
using UnityEngine;

namespace BreadFlip.Movement
{
    public class Toaster : MonoBehaviour
    {
        [SerializeField] private Transform _toastPosition;
        [SerializeField] private Transform _handle;
        [SerializeField] private Transform _maxHandleHeightPosition;
        
        private Vector3 _defaultHandlePosition;
        private float _maxHandleHeight;

        public Transform ToastPosition => _toastPosition;
        public Transform Handle => _handle;

        private void Start()
        {
            _defaultHandlePosition = _handle.transform.position;
            _maxHandleHeight = _maxHandleHeightPosition.position.y;
        }

        public void SetHandlePosition(float getForcePercent)
        {
            //_handle.transform.localPosition += _defaultHandlePosition + Vector3.up * (getForcePercent * _maxHandleHeight);
        }
    }
}
