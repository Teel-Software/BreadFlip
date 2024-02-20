using UnityEngine;
using UnityEngine.InputSystem;

namespace BreadFlip.Movement
{
    public class Swipe : MonoBehaviour
    {
        #region Events
        public delegate void StartTouch(Vector2 position, float time);
        public event StartTouch OnStartTouch;

        public delegate void EndTouch(Vector2 position, float time);
        public event EndTouch OnEndTouch;
        #endregion


        // [SerializeField] private PlayerControls playerControls;
        [SerializeField] private InputActionReference primaryTap;
        [SerializeField] private InputActionReference primaryPosition;
        private Camera mainCamera;

        private void OnEnable() {
            mainCamera = Camera.main;
            primaryTap.action.Enable();
            primaryPosition.action.Enable();
        }

        private void OnDisable() {
            primaryTap.action.Disable();
            primaryPosition.action.Disable();
        }

        void Start()
        {
            primaryTap.action.started += ctx => StartTouchPrimary(ctx);
            primaryTap.action.canceled += ctx => EndTouchPrimary(ctx);
        }

        private void EndTouchPrimary(InputAction.CallbackContext ctx)
        { 
            if (OnEndTouch != null)
            {
                OnEndTouch(
                    ScreenToWorldCoordinates(
                        mainCamera, 
                         primaryPosition.action.ReadValue<Vector2>()), 
                    (float)ctx.time);
                Debug.LogWarning("END touch");
            }
        }

        private void StartTouchPrimary(InputAction.CallbackContext ctx)
        {
            if (OnStartTouch != null)
            {
                OnStartTouch(
                    ScreenToWorldCoordinates(
                        mainCamera, 
                         primaryPosition.action.ReadValue<Vector2>()), 
                    (float)ctx.startTime);
                Debug.LogWarning("START touch");
            }
        }

        private Vector3 ScreenToWorldCoordinates(Camera camera, Vector3 position)
        {
            position.z = camera.nearClipPlane;
            return camera.ScreenToWorldPoint(position);
        }
    }
}
