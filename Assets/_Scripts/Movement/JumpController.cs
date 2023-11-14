using System.Collections;
using BreadFlip.Entities;
using BreadFlip.Sound;
using BreadFlip.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BreadFlip.Movement
{
    public class JumpController : MonoBehaviour
    {
        [SerializeField] private Toast _toast;
        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private TrajectoryRenderer _trajectoryRenderer;

        [SerializeField] private GameObject mainMenu;

        [Header("Audio")]
        [SerializeField] private SoundManager _soundManager;
        
        private bool firstSoundPlayed;
        private bool secondSoundPlayed;

        private bool _isDoubleJumpPressed;
        private float _startTime;
        private bool _inToaster;
        private float _rotationSpeed = 563f;
        
        private IEnumerator _playRotateAnimation;
        private Vector3 _rotateAxis;
        private int _speedMultiplicator = 1;

        private const float _MAX_TIME = 1.3f;
        public Toaster CurrentToaster { get; set; }
        public Toast Toast => _toast;

        private void OnValidate()
        {
            _rigidbody ??= gameObject.GetComponent<Rigidbody>();
            _trajectoryRenderer ??= gameObject.GetComponentInChildren<TrajectoryRenderer>();
        }

        public void Reset()
        {
            _isDoubleJumpPressed = false;
            _inToaster = true;
            
            firstSoundPlayed = false;
            secondSoundPlayed = false;


            ResetRotation();
        }

        private void Update()
        {
            if (!_inToaster)
                TryDoubleJump();
            else PrepareToJump();
        }

        private void PrepareToJump()
        {
            if (!mainMenu.activeSelf)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _startTime = Time.time;
                }

                if (Input.GetMouseButton(0) && _startTime != 0)
                {
                    CurrentToaster.SetHandlePosition(GetForcePercent());
                    _trajectoryRenderer.ShowTrajectory(gameObject.transform.position, GetForceVector());

                    // звук старта при прыжке, играет один раз
                    if (!firstSoundPlayed)
                    {
                        _soundManager.PlayJumpFirst();
                        firstSoundPlayed = true;
                    }
                }

                if (Input.GetMouseButtonUp(0) && _startTime != 0)
                {
                    var forceVector = GetForceVector();

                    if (forceVector.magnitude > 4)
                    {
                        CurrentToaster.JumpUp();

                        _rigidbody.AddForce(forceVector, ForceMode.Impulse);
                        _trajectoryRenderer.ClearTrajectory();

                        _playRotateAnimation = PlayRotateAnimation();
                        StartCoroutine(_playRotateAnimation);

                        _inToaster = false;

                        // звук вылета хлеба из тостера, играет один раз
                        if (!secondSoundPlayed)
                        {
                            _soundManager.PlayJumpSecond();
                            secondSoundPlayed = true;
                        }
                    }

                    _startTime = 0;
                }
            }
        }

        private void TryDoubleJump()
        {
            if (Input.GetMouseButtonDown(0) && !_isDoubleJumpPressed && !UiManager.pauseButtonPressed)
            {
                if (_rigidbody.velocity.y < 0)
                    _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

                _rigidbody.AddForce(new Vector3(0, 10f, 0f), ForceMode.Impulse);

                InverseRotation();

                _isDoubleJumpPressed = true;

                // начать double jump sound
                _soundManager.PlayDoubleJump();
            }

            if (Input.GetMouseButtonUp(0) && _isDoubleJumpPressed && _rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity =
                    new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y / 2f, _rigidbody.velocity.z);

                // остановить double jump sound
                _soundManager.StopDoubleJumpSound();
            }
        }

        private void ResetRotation()
        {
            StopRotation();
            Toast.ModelTransform.localRotation = Quaternion.identity;

            _speedMultiplicator = 1;
            
            _rotateAxis = GetRandomAxis();
        }
        
        private void InverseRotation()
        {
            StopRotation();
            
            _rotateAxis = -_rotateAxis;
            _playRotateAnimation = PlayRotateAnimation();
            _speedMultiplicator = 1;
            StartCoroutine(_playRotateAnimation);
        }

        private IEnumerator PlayRotateAnimation()
        {
            while (true)
            {
                Toast.ModelTransform.Rotate(_rotateAxis, _rotationSpeed * _speedMultiplicator * Time.deltaTime);
                yield return null;
            }
        }

        private Vector3 GetRandomAxis()
        {
            return new Vector3(
                Random.Range(0, 2) * 2 - 1,
                Random.Range(0, 2) * 2 - 1,
                Random.Range(0, 2) * 2 - 1
            );
        }

        private Vector3 GetForceVector()
        {
            var direction = -transform.forward;
            var force = GetForcePercent();

            const int c = 10;
            var newVector = new Vector3(direction.x * force, force, direction.z * force) * c;
            return newVector;
        }

        private float GetForcePercent()
        {
            if (_startTime == 0) return 0;

            var percent = (Time.time - _startTime) / _MAX_TIME;
            if ((int)percent % 2 == 0) return percent - (int)percent;
            return 1 - (percent - (int)percent);
        }

        public void UnlockPhysicsRotation()
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
            Toast.ModelCollider.enabled = true;
        }

        public void StopRotation()
        {
            if (_playRotateAnimation != null) StopCoroutine(_playRotateAnimation);
        }
    }
}