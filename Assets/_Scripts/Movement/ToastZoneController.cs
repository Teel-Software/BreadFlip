using BreadFlip.Sound;
using BreadFlip.UI;
using System;
using System.Collections;
using System.Reflection;
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

        private IEnumerator _waitingCoroutine;

        [Header("Audio")]
        [SerializeField] private SoundManager _soundManager;
        
        public bool startedInToaster;

        public event Action OnCollidedCoinAction;
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

        public void OnCollideCoin(GameObject coinObj)
        {
            coinObj.SetActive(false);

            OnCollidedCoinAction?.Invoke();
            _soundManager.PlayRewardSound();
        }

        public void OnCollideToaster(GameObject toasterObj)
        {
            if (_collidedBadThing) 
            {
                Debug.LogWarning("задели пол перед тостером");
                Debug.LogWarning($"{_waitingCoroutine}");
                StopCoroutine(_waitingCoroutine);
                _waitingCoroutine = null;
            }
            
            // if (!_collidedBadThing)
            // {
                _collidedToaster = true;

                Debug.Log(MethodBase.GetCurrentMethod());

                var toaster = toasterObj.GetComponent<Toaster>();
                if (!toaster) return;
                _jumpController.CurrentToaster = toaster;

                toaster.SetToast(transform, _jumpController.SkinChanger.CurrentSkin);

                _rigidbody.velocity = Vector3.zero;
                _jumpController.Reset();
                _jumpController.ResetConstraints();

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
            // }
        }

        public void OnCollideBadThing(GameObject badThing)
        {
            if (!_collidedToaster)
            {
                Debug.Log(MethodBase.GetCurrentMethod());
                
                PlayCrumbs();
                _jumpController.UnlockPhysicsRotation();
                _jumpController.StopRotation();

                _soundManager.PlayFailedSound();

                if (_waitingCoroutine == null)
                {
                    _waitingCoroutine = WaitBeforeInvokeBadCollide();
                    StartCoroutine(_waitingCoroutine);
                }
                
                
                _collidedBadThing = true;
            }
        }

        private IEnumerator WaitBeforeInvokeBadCollide()
        {
            yield return new WaitForSeconds(2f);
            OnCollidedBadThing?.Invoke();
        }

        public void OnExitFromCollider(GameObject toasterObj)
        {
            Debug.Log(MethodBase.GetCurrentMethod());
            
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