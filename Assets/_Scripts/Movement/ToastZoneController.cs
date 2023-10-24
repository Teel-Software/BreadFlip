using System;
using UnityEngine;

namespace BreadFlip.Movement
{
    public class ToastZoneController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private JumpController _jumpController;

        //[SerializeField] private Timer _timer;

        public event Action OnCollidedToaster;
        public event Action OnCollidedBadThing;
        public event Action OnColliderExit;

        
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

        public void OnExitFromCollider(GameObject toasterObj)
        {
            OnColliderExit?.Invoke();
        }
    }
}