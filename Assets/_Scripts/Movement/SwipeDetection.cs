using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadFlip.Movement
{
    public class SwipeDetection : MonoBehaviour
    {
        public static event Action SwipeDownEvent;
        public static event Action SwipeUpEvent;

        private Vector2 _firstPos;
        private Vector2 _swipeDelta;

        private float _deadzone = 80f;

        private bool _isSwiping;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isSwiping = true;
                _firstPos = Input.mousePosition;
            }

            CheckSwipe();
        }

        private void CheckSwipe()
        {
            _swipeDelta = Vector2.zero;

            if (_isSwiping)
            {
                _swipeDelta = (Vector2)Input.mousePosition - _firstPos;
            }

            if (_swipeDelta.magnitude > _deadzone)
            {
                // свайп вниз
                if (MathF.Abs(_swipeDelta.y) > Mathf.Abs(_swipeDelta.x) &&
                    _swipeDelta.y < 0)
                {
                    SwipeDownEvent?.Invoke();
                    ResetSwipe();
                }
                
                // свайп вверх
                else if (MathF.Abs(_swipeDelta.y) > Mathf.Abs(_swipeDelta.x) &&
                    _swipeDelta.y > 0)
                {
                    SwipeUpEvent?.Invoke();
                    ResetSwipe();
                }

            }
        }

        private void ResetSwipe()
        {
            _isSwiping = false;

            _firstPos = Vector2.zero;
            _swipeDelta = Vector2.zero;
        }
    }
}
