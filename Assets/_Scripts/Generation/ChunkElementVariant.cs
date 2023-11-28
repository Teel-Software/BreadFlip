using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BreadFlip.Generation
{
    public class ChunkElementVariant : MonoBehaviour
    {
        [SerializeField] private EntryZoneComponent[] _entryZoneComponents;
        [SerializeField] private ChunkType _chunkType = ChunkType.Table;
        [SerializeField] private List<Transform> _propsPoints;
        
        public GameObject StartSpawnPoint;
        public GameObject EndSpawnPoint;

        public AnimationCurve ChanceFromDistance;

        public GameObject Visual;

        public IReadOnlyCollection<EntryZoneComponent> EntryZoneComponents => _entryZoneComponents;
        public ChunkType ChunkType => _chunkType;
        public IReadOnlyCollection<Transform> PropsPoints => _propsPoints;

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