using System;
using UnityEngine;

namespace BreadFlip.Movement
{
    public class ToastZoneController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private JumpController _jumpController;

        public event Action OnCollidedToaster;
        public event Action OnCollidedBadThing;
        
        public void OnCollideToaster(GameObject toasterObj)
        {
            
            var toaster = toasterObj.GetComponent<Toaster>();
            if (!toaster) return;
            _jumpController.CurrentToaster = toaster;

            var newPos = toaster.ToastPosition.position;
            transform.position = newPos;
            
            _rigidbody.velocity = Vector3.zero;
            _jumpController.Reset();
            
            OnCollidedToaster?.Invoke();
        }

        public void OnCollideBadThing(GameObject badThing)
        {
            _jumpController.enabled = false;
            OnCollidedBadThing?.Invoke();
        }
    }
}