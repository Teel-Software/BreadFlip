using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BreadFlip.Movement
{
    public class SwipeDetection : MonoBehaviour
    {
        private Vector2 _startTouchPosition;
        private Vector2 _movedTouchPosition;

        private const float SWIPE_DISTANCE = 0.5f;

        private static bool _touchMoved;

        public static bool TouchMoved
        {
            get {return _touchMoved;}
        }

        public static event Action SwipeDownEvent;

        public static bool SwipeEnabled {get; set;}

        public static float SwipeDistance => SWIPE_DISTANCE;

        private static bool _swipeWas;

        private SwipeCondition _swipeCondition;

        [SerializeField] private InputActionReference swipeTapReference;
        [SerializeField] private InputActionReference swipePositionReference;

        private static float y_difference_prv;

        public static float Y_diff => y_difference_prv;


        private void OnEnable() {
            swipeTapReference.action.Enable();
            swipeTapReference.action.Enable();
        }

        private void OnDisable() {
            swipeTapReference.action.Disable();
            swipePositionReference.action.Disable();
        }

        // public void SetChangingY(InputAction.CallbackContext context)
        // {
        //     y_difference_prv = _startTouchPosition.y - _movedTouchPosition.y;
        //     if (Math.Abs(y_difference_prv) > SWIPE_DISTANCE && _movedTouchPosition.y < _startTouchPosition.y)
        //     {
        //         _touchMoved = true;
        //     }
        // }

        public void DetectSwipe (InputAction.CallbackContext context)
        {
            // Debug.Log($"READ VALUE: {swipePositionReference.action.ReadValue<Vector2>()}");
            if (context.started)
            {
                // Debug.Log("STARTEEEED");
                // _swipeCondition = SwipeCondition.Started;
                _startTouchPosition = swipePositionReference.action.ReadValue<Vector2>();
            }

            else if (context.performed)
            {
                // Debug.Log("END");
                // _swipeCondition = SwipeCondition.Performed;
                _movedTouchPosition = swipePositionReference.action.ReadValue<Vector2>();

                if (SwipeEnabled && !_swipeWas)
                {
                    if (Math.Abs(_startTouchPosition.y - _movedTouchPosition.y) > SWIPE_DISTANCE && _movedTouchPosition.y < _startTouchPosition.y)
                    {
                        _touchMoved = true;
                        SwipeDownEvent?.Invoke();
                        _swipeWas = true;
                    }
                    else
                    {
                        _touchMoved = false;
                    }
                }
            }
        }

        public static void Reset() {
            SwipeEnabled = false;
            _swipeWas = false;
            _touchMoved = false;
        }

//         private void Update() 
//         {
//             // if (SwipeEnabled && !_swipeWas)
//             // {
//             //     if (Mouse.current.leftButton.wasPressedThisFrame /* Input.GetMouseButtonDown(0) */)
//             //     {
//             //         _startTouchPosition = Mouse.current.position.ReadValue(); // Input.mousePosition;
//             //     }

//             //     if (Mouse.current.leftButton.isPressed /* Input.GetMouseButton(0) */)
//             //     {
//             //         _movedTouchPosition = Mouse.current.position.ReadValue(); // Input.mousePosition;
//             //         if (_startTouchPosition.y - _movedTouchPosition.y > 0.05f && _movedTouchPosition.y < _startTouchPosition.y)
//             //         {
//             //             _touchMoved = true;
//             //             SwipeDownEvent?.Invoke();
//             //             _swipeWas = true;
//             //         }
//             //         else
//             //         {
//             //             _touchMoved = false;
//             //         }
//             //     }
//             // }

// #region Touch
// //             if (Input.touchCount > 0)
// //             {
// //                 if (Input.GetTouch(0).phase == TouchPhase.Began)
// //                 {
// //                     _startTouchPosition = Input.GetTouch(0).position;
// //                 }

// //                 if (Input.GetTouch(0).phase == TouchPhase.Moved)
// //                 {
// //                     _movedTouchPosition = Input.GetTouch(0).position;
// //                     if (Math.Abs(_movedTouchPosition.y - _startTouchPosition.y) > 0.05f)
// //                     {
// //                         _touchMoved = true;
// //                         SwipeDownEvent?.Invoke();
// //                     }
// //                     else
// //                     {
// //                         _touchMoved = false;
// //                     }
// //                 }

// //                 // if (Input.GetTouch(0).phase == TouchPhase.Ended)
// //                 // {
// //                 //     _endTouchPosition = Input.GetTouch(0).position;

// //                 //     if (_endTouchPosition.y < _startTouchPosition.y && (_startTouchPosition.y - _endTouchPosition.y > 2f))
// //                 //     {
// //                 //         // var currentSwipe = (Vector2)Input.mousePosition - _startTouchPosition;
// // 			    //         // Debug.Log(currentSwipe+", "+currentSwipe.normalized);
// //                 //         // _jumpController.JumpDown();
// //                 //         SwipeDownEvent?.Invoke();
// //                 //     }
// //                 // }
// //             }
// #endregion
//         }
    }
}
