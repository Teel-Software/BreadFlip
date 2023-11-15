using System;
using UnityEngine;
using UnityEngine.Events;

namespace BreadFlip.Generation
{
    [RequireComponent(typeof(Collider))]
    public class EntryZoneComponent : MonoBehaviour
    {
        [SerializeField] private Collider _triggerZone;
        [SerializeField] private UnityGameObjectEvent _onEnteredZone;
        [SerializeField] private UnityGameObjectEvent _onExitZone;

        [SerializeField] private bool _activeHighlight = true;
    
        [SerializeField] private bool _isToaster;
        [SerializeField] private Color _color = Color.green;

        private bool IsZoneTrigger => _triggerZone.isTrigger;
        public bool IsToaster => _isToaster;


        public void SetZoneEnteredEvent(UnityGameObjectEvent myEvent)
        {
            _onEnteredZone = myEvent;
        }

        public void SetZoneExitEvent(UnityGameObjectEvent exitEvent)
        {
            _onExitZone = exitEvent;
        }

        private void Awake()
        {
            if (_triggerZone == null)
                _triggerZone = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!IsZoneTrigger)
            {
                _onEnteredZone.Invoke(gameObject);
                Debug.Log("Collision");
            }
        }

        //private void OnCollisionExit(Collision collision)
        //{
        //    if (!IsZoneTrigger)
        //    {
        //        _onExitZone.Invoke(gameObject);
        //        Debug.Log("Collision");
        //    }
        //}

        private void OnTriggerEnter(Collider other)
        {
            if (IsZoneTrigger)
            {
                _onEnteredZone.Invoke(gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsZoneTrigger)
            {
                _onExitZone.Invoke(gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            if (_triggerZone != null && _activeHighlight)
            {
                Gizmos.color = _color;
                var winZoneBounds = _triggerZone.bounds;
                if (winZoneBounds.size.y == 0)
                    winZoneBounds.size += Vector3.up *.1f;
            
                Gizmos.DrawCube(winZoneBounds.center, winZoneBounds.size * 1.01f);
            }
        }
    }

    [Serializable]
    public class UnityGameObjectEvent : UnityEvent<GameObject>
    {
    }
}