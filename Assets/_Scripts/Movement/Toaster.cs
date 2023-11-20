using System;
using BreadFlip.Entities;
using DG.Tweening;
using UnityEngine;

namespace BreadFlip.Movement
{
    public class Toaster : MonoBehaviour
    {
        [SerializeField] private Transform _toastPosition;
        [SerializeField] private Transform _handle;
        
        [SerializeField] private Transform _minHandleHeightPosition;
        [SerializeField] private Transform _maxHandleHeightPosition;

        private Vector3 _minHandlePosition;
        private Vector3 _maxHandlePosition;
        
        private Transform _toast;
        private Transform _toastModel;
        private float _modelUpOffset;

        private Transform ToastPosition => _toastPosition;

        private void Start()
        {
            var handlePosition = _handle.position;
            
            _minHandlePosition = new Vector3(handlePosition.x, _minHandleHeightPosition.position.y, handlePosition.z);
            _maxHandlePosition = new Vector3(handlePosition.x, _maxHandleHeightPosition.position.y, handlePosition.z);

            _handle.position = _maxHandlePosition;
        }
        
        public void SetToast(Transform toastMain, Toast toastModel)
        {
            _toast = toastMain;
            _toast.transform.position = ToastPosition.position;

            _modelUpOffset = toastModel.ModelUpOffset;

            _toastModel = toastModel.ModelTransform;
            _toastModel.localPosition += Vector3.up * _modelUpOffset;
        }

        public void SetHandlePosition(float getForcePercent)
        {
            _handle.position = Vector3.Lerp(_maxHandlePosition, _minHandlePosition, getForcePercent);

            var offset = _maxHandlePosition.y - _handle.position.y;
            _toastModel.localPosition = Vector3.down * (offset - _modelUpOffset);
        }

        public void JumpUp()
        {
            _toast = null;

            _toastModel.localPosition = Vector3.zero;
            _handle.DOMoveY(_maxHandlePosition.y, .25f).SetEase(Ease.OutElastic);
        }
    }
}
