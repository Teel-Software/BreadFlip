using BreadFlip.Sound;
using BreadFlip.UI;
using System;
using UnityEngine;

namespace BreadFlip.Movement
{
    public class ToastZoneController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private JumpController _jumpController;

        [SerializeField] private ParticleSystem _smokeToasterPrefab;
        [SerializeField] private ParticleSystem _crumbsToastPrefab;
        [SerializeField] private ParticleSystem _smokeSmokeDeadPrefab;

        [Header("Audio")]
        [SerializeField] private SoundManager _soundManager;
        public bool startedInToaster;

        public event Action OnCollidedToaster;
        public event Action OnCollidedBadThing;
        public event Action OnColliderExit;

        private bool _collidedToaster;
        private bool _collidedBadThing;

        private void Start()
        {
            _collidedBadThing = false;
            _collidedToaster = false;
            startedInToaster = true;
            Timer.TimeOvered += PlayDeadSmoke;
        }

        private void OnDestroy()
        {
            Timer.TimeOvered -= PlayDeadSmoke;
        }

        public void OnCollideToaster(GameObject toasterObj)
        {
            if (!_collidedBadThing)
            {
                _collidedToaster = true;

                var toaster = toasterObj.GetComponent<Toaster>();
                if (!toaster) return;
                _jumpController.CurrentToaster = toaster;

                var newPos = toaster.ToastPosition.position;
                transform.position = newPos;

                _rigidbody.velocity = Vector3.zero;
                _jumpController.Reset();

                PlaySmoke();
                PlayCrumbs();

                OnCollidedToaster?.Invoke();

                if (!startedInToaster)
                {
                    _soundManager.PlayLandedInToasterSound();
                }
                else
                {
                    startedInToaster = false;
                }
            }
        }

        public void OnCollideBadThing(GameObject badThing)
        {
            _collidedBadThing = true;
            PlayCrumbs();
            _jumpController.UnlockPhysicsRotation();
            _jumpController.StopRotation();
            
            _jumpController.enabled = false;
            OnCollidedBadThing?.Invoke();

            _soundManager.PlayFailedSound();
        }

        public void OnExitFromCollider(GameObject toasterObj)
        {
            PlaySmoke();
            PlayCrumbs();
            OnColliderExit?.Invoke();
            _collidedToaster = false;
            _collidedBadThing = false;
        }

        private void PlayCrumbs()
        {
            Instantiate(_crumbsToastPrefab, transform.position, Quaternion.identity);
        }

        private void PlaySmoke()
        {
            Instantiate(_smokeToasterPrefab, transform.position, Quaternion.identity);
        }

        public void PlayDeadSmoke()
        {
            Instantiate(_smokeSmokeDeadPrefab, transform.position, Quaternion.identity);
        }
    }
}