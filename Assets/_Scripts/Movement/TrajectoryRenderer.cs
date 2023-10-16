using System;
using UnityEngine;

namespace BreadFlip.Movement
{
    public class TrajectoryRenderer : MonoBehaviour
    {
        [SerializeField] private float _groundLevel = -9.6f;
        [SerializeField] private LineRenderer _lineRendererComponent;

        private void OnValidate()
        {
            _lineRendererComponent ??= GetComponent<LineRenderer>();
        }

        public void ShowTrajectory(Vector3 origin, Vector3 speed)
        {
            var points = new Vector3[100];
            
            _lineRendererComponent.enabled = true;
            _lineRendererComponent.positionCount = points.Length;

            for (var i = 0; i < points.Length; i++)
            {
                var time = i * 0.1f;

                points[i] = origin + speed * time + Physics.gravity * time * time / 2f;

                if (points[i].y < _groundLevel)
                {
                    _lineRendererComponent.positionCount = i + 1;
                    break;
                }
            }

            _lineRendererComponent.SetPositions(points);
        }

        public void ClearTrajectory()
        {
            _lineRendererComponent.enabled = false;
        }
    }
}
