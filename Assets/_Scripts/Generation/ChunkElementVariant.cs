using System;
using System.Collections.Generic;
using UnityEngine;

namespace BreadFlip.Generation
{
    public class ChunkElementVariant : MonoBehaviour
    {
        public GameObject StartSpawnPoint;
        public GameObject EndSpawnPoint;

        public AnimationCurve ChanceFromDistance;

        public GameObject Visual;

        [SerializeField] private EntryZoneComponent[] _entryZoneComponents;

        public IReadOnlyCollection<EntryZoneComponent> EntryZoneComponents => _entryZoneComponents;

        private void OnValidate()
        {
            if (EntryZoneComponents == null || EntryZoneComponents.Count == 0)
            {
                var zones = GetComponentsInChildren<EntryZoneComponent>();
                _entryZoneComponents = zones;
            }
        }
    }
}