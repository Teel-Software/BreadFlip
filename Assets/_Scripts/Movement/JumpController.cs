using UnityEngine;

namespace BreadFlip.Movement
{
    public class JumpController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private TrajectoryRenderer _trajectoryRenderer;

        private bool _isDoubleJumpPressed;
        private float _startTime;
        private bool _inToaster;

        private const float _MAX_TIME = 1.3f;

        private void OnValidate()
        {
            _rigidbody ??= gameObject.GetComponent<Rigidbody>();
            _trajectoryRenderer ??= gameObject.GetComponentInChildren<TrajectoryRenderer>();
        }

        public void Reset()
        {
            _isDoubleJumpPressed = false;
            _inToaster = true;
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
                _trajectoryRenderer.ShowTrajectory(gameObject.transform.position, GetForceVector());
            }

            if (Input.GetMouseButtonUp(0) && _startTime != 0)
            {
                var forceVector = GetForceVector();

                if (forceVector.magnitude > 1)
                {
                    _rigidbody.AddForce(forceVector, ForceMode.Impulse);
                    _trajectoryRenderer.ClearTrajectory();
                    
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
                _isDoubleJumpPressed = true;
            }

            if (Input.GetMouseButtonUp(0) && _isDoubleJumpPressed && _rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y / 2f, _rigidbody.velocity.z);
            }
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
    }
}