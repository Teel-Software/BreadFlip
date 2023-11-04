using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BreadFlip.Movement
{
    public class JumpController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _modelTransform;

        [SerializeField] private BoxCollider _collisionCollider;
        [SerializeField] private BoxCollider _modelCollider;

        [SerializeField] private TrajectoryRenderer _trajectoryRenderer;
        
        private bool _isDoubleJumpPressed;
        private float _startTime;
        private bool _inToaster;
        private float _rotationSpeed = 563f;
        
        private IEnumerator _playRotateAnimation;
        private Vector3 _rotateAxis;
        private int _speedMultiplicator = 1;

        private const float _MAX_TIME = 1.3f;
        public Toaster CurrentToaster { get; set; }

        private void OnValidate()
        {
            _rigidbody ??= gameObject.GetComponent<Rigidbody>();
            _animator ??= gameObject.GetComponent<Animator>();
            _trajectoryRenderer ??= gameObject.GetComponentInChildren<TrajectoryRenderer>();

            if (_modelCollider) _modelCollider.enabled = false;
        }

        public void Reset()
        {
            _isDoubleJumpPressed = false;
            _inToaster = true;
            
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
            if (Input.GetMouseButtonDown(0))
            {
                _startTime = Time.time;
            }

            if (Input.GetMouseButton(0) && _startTime != 0)
            {
                //CurrentToaster.SetHandlePosition(GetForcePercent());
                _trajectoryRenderer.ShowTrajectory(gameObject.transform.position, GetForceVector());
            }

            if (Input.GetMouseButtonUp(0) && _startTime != 0)
            {
                var forceVector = GetForceVector();

                if (forceVector.magnitude > 2)
                {
                    _rigidbody.AddForce(forceVector, ForceMode.Impulse);
                    _trajectoryRenderer.ClearTrajectory();

                    _playRotateAnimation = PlayRotateAnimation();
                    StartCoroutine(_playRotateAnimation);

                    CurrentToaster.JumpUp();

                    _inToaster = false;
                }

                _startTime = 0;
            }
        }

        private void TryDoubleJump()
        {
            if (Input.GetMouseButtonDown(0) && !_isDoubleJumpPressed)
            {
                if (_rigidbody.velocity.y < 0)
                    _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

                _rigidbody.AddForce(new Vector3(0, 10f, 0f), ForceMode.Impulse);

                InverseRotation();

                _isDoubleJumpPressed = true;
            }

            if (Input.GetMouseButtonUp(0) && _isDoubleJumpPressed && _rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity =
                    new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y / 2f, _rigidbody.velocity.z);
            }
        }

        private void ResetRotation()
        {
            StopRotation();
            _modelTransform.localRotation = Quaternion.identity;

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
                _modelTransform.Rotate(_rotateAxis, _rotationSpeed * _speedMultiplicator * Time.deltaTime);
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
            _modelCollider.enabled = true;
            _collisionCollider.enabled = false;
        }

        public void StopRotation()
        {
            if (_playRotateAnimation != null) StopCoroutine(_playRotateAnimation);
        }
    }
}