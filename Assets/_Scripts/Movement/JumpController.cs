using System;
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

        [Header("Colors")]
        [SerializeField] private Material lineRedererMaterial;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color redColor;
        

        //private bool firstSoundPlayed;
        private bool secondSoundPlayed;

        private bool _isDoubleJumpPressed;
        private float _startTime;
        private bool _inToaster;
        private float _rotationSpeed = 563f;
        private bool _canStartJump;
        
        private IEnumerator _playRotateAnimation;
        private Vector3 _rotateAxis;
        private int _speedMultiplicator = 1;
        private bool _canDoubleJump;
        private bool _playerFailed;
        private Vector3 forceVector;

        private bool bylo;

        private const float _MAX_TIME = 1.3f;
        private const float _MAGNITUDE = 5f;
        public Toaster CurrentToaster { get; set; }
        public Toast Toast => _toast;

        public Rigidbody Rigidbody => _rigidbody;

        public bool CanDoubleJump 
        {
            get {return _canDoubleJump; }
            set {_canDoubleJump = value;}
        }

        private void Start()
        {
            Timer.TimeOvered += () => _canStartJump = false;

            gameObject.GetComponent<ToastZoneController>().OnCollidedBadThing += () => _playerFailed = true;

            SwipeDetection.SwipeDownEvent += JumpDown;
        }
        
        private void OnValidate()
        {
            _rigidbody ??= gameObject.GetComponent<Rigidbody>();
            _trajectoryRenderer ??= gameObject.GetComponentInChildren<TrajectoryRenderer>();
        }

        public void Reset()
        {
            _isDoubleJumpPressed = false;
            _inToaster = true;

            _canStartJump = true;

            secondSoundPlayed = false;
            _canDoubleJump = false;

            SwipeDetection.Reset();

            transform.rotation = Quaternion.Euler(new Vector3 (0f, 90f, 0));

            ResetAnimRotation();
        }

        private void Update()
        {
            // if (_inToaster && !bylo)
            // {
            //     Debug.LogWarning("Sleep");
            //     bylo = true;
            // }
            // if (!_inToaster)
            // {   
            //     bylo = false;
            //     Debug.LogWarning($"Update: {Rigidbody.velocity}");
            // }
            
            if (!_inToaster)
                TryDoubleJump();
            else PrepareToJump();
        }

        private void PrepareToJump()
        {
            SwipeDetection.SwipeEnabled = false;
            if (!mainMenu.activeSelf && _canStartJump)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _startTime = Time.time;
                }

                if (Input.GetMouseButton(0) && _startTime != 0)
                {
                    forceVector = GetForceVector();
                    CurrentToaster.SetHandlePosition(GetForcePercent());

                    lineRedererMaterial.color = defaultColor; //Color.Lerp(defaultColor, redColor, .1f);
                    _trajectoryRenderer.ShowTrajectory(gameObject.transform.position, forceVector);

                    
                    if (forceVector.magnitude > _MAGNITUDE)
                    {
                        lineRedererMaterial.color = defaultColor; //Color.Lerp(defaultColor, redColor, .1f);
                    }
                    else
                    {
                        lineRedererMaterial.color = redColor; //Color.Lerp(redColor, defaultColor, .1f);
                    }
                }

                if (Input.GetMouseButtonUp(0) && _startTime != 0)
                {
                    // var forceVector = GetForceVector();
                    
                    if (forceVector.magnitude > _MAGNITUDE && _rigidbody.velocity.y >= 0f)
                    {
                        StartCoroutine(Wait(.45f, () => _canDoubleJump = true));
                     
                        CurrentToaster.JumpUp();

                        // _rigidbody.AddForce(forceVector, ForceMode.Impulse);
                        _rigidbody.velocity += forceVector;
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
                    else
                    {
                        _trajectoryRenderer.ClearTrajectory();
                    }

                    _startTime = 0;
                }
            }
        }

        private IEnumerator Wait(float time, Func<bool> action)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }

        private void TryDoubleJump()
        {
            if (!_canDoubleJump) return;
            SwipeDetection.SwipeEnabled = true;
            if (Input.GetMouseButtonDown(0) && !_isDoubleJumpPressed && !UiManager.pauseButtonPressed && !SwipeDetection.TouchMoved)
            {
                    if (_rigidbody.velocity.y < 0)
                        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

                    _rigidbody.AddForce(new Vector3(0, 10f, 0f), ForceMode.Impulse);

                    InverseRotation();

                    _isDoubleJumpPressed = true;

                    // начать double jump sound
                    if (!_playerFailed)
                        _soundManager.PlayDoubleJump();
            }

            else if (SwipeDetection.TouchMoved)
            {
                _soundManager.StopJumpSound();
            }

            if (Input.GetMouseButtonUp(0) && _isDoubleJumpPressed && _rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity =
                    new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y / 2f, _rigidbody.velocity.z);

            
                // остановить double jump sound
                _soundManager.StopJumpSound();
            }
        }

        private IEnumerator WaitForSwipe()
        {
            yield return new WaitForSeconds(0.4f);
        }

        private void ResetAnimRotation()
        {
            StopRotation();
            Toast.ModelCollider.transform.localRotation = Quaternion.identity;

            _speedMultiplicator = 1;
            
            _rotateAxis = GetRandomAxis();
        }
        
        private void InverseRotation()
        {
            StopRotation();
            
            _rotateAxis = -_rotateAxis;
            _playRotateAnimation = PlayRotateAnimation(true);
            _speedMultiplicator = 1;
            StartCoroutine(_playRotateAnimation);
        }

        private IEnumerator PlayRotateAnimation(bool ignoreDelay = false)
        {
            if (!ignoreDelay) yield return new WaitForSeconds(.15f);
            while (true)
            {
                Toast.ModelCollider.transform.Rotate(_rotateAxis, _rotationSpeed * _speedMultiplicator * Time.deltaTime);
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
        }

        public void ResetConstraints()
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
        }

        public void StopRotation()
        {
            if (_playRotateAnimation != null) StopCoroutine(_playRotateAnimation);
        }

        public void JumpDown()
        {
            if (_rigidbody != null && !_inToaster)
            {
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x / 1.5f, 0f, _rigidbody.velocity.z);
            
                _rigidbody.AddForce(new Vector3(0f, -15f, 0), ForceMode.Impulse);
            }
        }
    }
}