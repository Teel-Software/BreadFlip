using BreadFlip.Movement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BreadFlip.Generation
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private ChunkElementVariant[] _tables;
        [SerializeField] private ChunkElementVariant[] _tableVariants;
    
        public GameObject StartSpawnPoint;
        public GameObject EndSpawnPoint;

        public AnimationCurve ChanceFromDistance;

        public void ReplacePrefabInChunk()
        {
            if (_tables.Length == 0 || _tableVariants.Length == 0) return;
            
            Vector3 lastTableEndPosition;

            for (var i = 1; i < _tables.Length; i++)
            {
                var replaceableTable = _tables[i];
                var previousTable = _tables[i-1];
            
                var newTable = Instantiate(_tableVariants[Random.Range(0, _tableVariants.Length)], transform);

                lastTableEndPosition = previousTable.EndSpawnPoint.transform.position;
                var newTableStartPosition = newTable.StartSpawnPoint.transform.localPosition;

                newTable.transform.position = lastTableEndPosition - newTableStartPosition;
                Destroy(replaceableTable.gameObject);

                _tables[i] = newTable;
            }
        
            lastTableEndPosition = _tables[^1].EndSpawnPoint.transform.position;
            EndSpawnPoint.transform.position = lastTableEndPosition;
        }

        public void RegisterEntryZones(ToastZoneController toastZoneController)
        {
            foreach (var table in _tables)
            {
                foreach (var entryZoneComponent in table.EntryZoneComponents)
                {
                    var zoneCallback = new UnityGameObjectEvent();
                    zoneCallback.AddListener(entryZoneComponent.IsToaster ? toastZoneController.OnCollideToaster : toastZoneController.OnCollideBadThing);
                    entryZoneComponent.SetZoneEnteredEvent(zoneCallback);

                    var zoneExitCallback = new UnityGameObjectEvent();
                    if (entryZoneComponent.IsToaster)   zoneExitCallback.AddListener(toastZoneController.OnExitFromCollider);
                    entryZoneComponent.SetZoneExitEvent(zoneExitCallback);
                }
            }
        }
    }
}
