using System;
using UnityEngine;

namespace BreadFlip.Movement
{
    public class SwipeDetection : MonoBehaviour
    {
        private Vector2 _startTouchPosition;
        private Vector2 _movedTouchPosition;

        private static bool _touchMoved;

        public static bool TouchMoved
        {
            get {return _touchMoved;}
        }

        public static event Action SwipeDownEvent;

        public static bool SwipeEnabled {get; set;}

        private static bool _swipeWas;

        public static void Reset() {
            SwipeEnabled = false;
            _swipeWas = false;
            _touchMoved = false;
        }

        // private JumpController _jumpController;

        // private void Start() {
        //     _jumpController = GetComponent<JumpController>();
        // }

        private void Update() 
        {
            if (SwipeEnabled && !_swipeWas)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _startTouchPosition = Input.mousePosition;
                }

                if (Input.GetMouseButton(0))
                {
                    _movedTouchPosition = Input.mousePosition;
                    if (_startTouchPosition.y - _movedTouchPosition.y > 1f && _movedTouchPosition.y < _startTouchPosition.y)
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

#region Touch
//             if (Input.touchCount > 0)
//             {
//                 if (Input.GetTouch(0).phase == TouchPhase.Began)
//                 {
//                     _startTouchPosition = Input.GetTouch(0).position;
//                 }

//                 if (Input.GetTouch(0).phase == TouchPhase.Moved)
//                 {
//                     _movedTouchPosition = Input.GetTouch(0).position;
//                     if (Math.Abs(_movedTouchPosition.y - _startTouchPosition.y) > 0.05f)
//                     {
//                         _touchMoved = true;
//                         SwipeDownEvent?.Invoke();
//                     }
//                     else
//                     {
//                         _touchMoved = false;
//                     }
//                 }

//                 // if (Input.GetTouch(0).phase == TouchPhase.Ended)
//                 // {
//                 //     _endTouchPosition = Input.GetTouch(0).position;

//                 //     if (_endTouchPosition.y < _startTouchPosition.y && (_startTouchPosition.y - _endTouchPosition.y > 2f))
//                 //     {
//                 //         // var currentSwipe = (Vector2)Input.mousePosition - _startTouchPosition;
// 			    //         // Debug.Log(currentSwipe+", "+currentSwipe.normalized);
//                 //         // _jumpController.JumpDown();
//                 //         SwipeDownEvent?.Invoke();
//                 //     }
//                 // }
//             }
#endregion
        }
    }
}
