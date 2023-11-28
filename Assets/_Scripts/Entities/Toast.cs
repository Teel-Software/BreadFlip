using System;
using UnityEngine;

namespace BreadFlip.Entities
{
    public class Toast : MonoBehaviour
    {
        [field:SerializeField] public Transform ModelTransform {get; private set;}
        [field: SerializeField] public float ModelUpOffset { get; private set; } = .1f;
        [field:SerializeField] public BoxCollider ModelCollider {get; private set;}
        
        [field : SerializeField] public Renderer PulpRenderer {get; private set;}
        [field : SerializeField] public Renderer CrustRenderer {get; private set;}

        private void OnValidate()
        {
            ModelTransform ??= transform.GetChild(0);
            ModelCollider ??= GetComponent<BoxCollider>();
            if (ModelCollider) ModelCollider.enabled = true;
        }
    }
}