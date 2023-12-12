using System;
using System.Collections.Generic;
using System.Linq;
using BreadFlip.Generation.Props;
using BreadFlip.Movement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BreadFlip.Generation
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private ChunkElementVariant[] _tables;
        [SerializeField] private ChunkElementVariant[] _tableVariants;

        [SerializeField] private EntryZoneComponent[] _entryZoneComponents;

        private List<PropsElement> _spawnedProps = new();
        
        public GameObject StartSpawnPoint;
        public GameObject EndSpawnPoint;

        public AnimationCurve ChanceFromDistance;

        private void OnValidate()
        {
            if (_entryZoneComponents is { Length: 0 })
                _entryZoneComponents = GetComponentsInChildren<EntryZoneComponent>();
        }

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

        public void SpawnProps(IReadOnlyCollection<PropsElement> propsPrefabs)
        {
            if (_tables.Length == 0 || propsPrefabs.Count == 0) return;
            
            foreach (var table in _tables)
            {
                var suitableProps = propsPrefabs
                    .Where(p => p.ChunkTypes.HasFlag((ChunkTypes)table.ChunkType))
                    .ToList();

                if (suitableProps.Count == 0) continue;
                
                var rndPropsIndex = Random.Range(0, suitableProps.Count);
                var rndPropsPositionIndex = Random.Range(0, table.PropsPoints.Count);

                var rndPropPoint = table.PropsPoints.ElementAt(rndPropsPositionIndex);

                var propsPrefab = suitableProps.ElementAt(rndPropsIndex);
                
                var rotation = Quaternion.identity;
                if (propsPrefab.CanRotate) rotation *= Quaternion.Euler(0, Random.Range(0, 360), 0);

                var spawnedPropsElement = Instantiate(propsPrefab, rndPropPoint.position, rotation, rndPropPoint);
                
                _spawnedProps.Add(spawnedPropsElement);
            }
        }

        public void RegisterEntryZones(ToastZoneController toastZoneController)
        {
            foreach (var table in _tables)
            {
                foreach (var entryZoneComponent in table.EntryZoneComponents)
                {
                    RegisterEntryZone(toastZoneController, entryZoneComponent);
                }
            }

            foreach (var entryZoneComponent in _entryZoneComponents)
            {
                RegisterEntryZone(toastZoneController, entryZoneComponent);
            }
        }

        private static void RegisterEntryZone(ToastZoneController toastZoneController, EntryZoneComponent entryZoneComponent)
        {
            var zoneCallback = new UnityGameObjectEvent();
            if (entryZoneComponent.IsToaster)
            {
                zoneCallback.AddListener(toastZoneController.OnCollideToaster);
            }
            else
            {
                if (entryZoneComponent.gameObject.tag == "Coin") zoneCallback.AddListener(toastZoneController.OnCollideCoin);
                else zoneCallback.AddListener(toastZoneController.OnCollideBadThing);
            }
            // zoneCallback.AddListener(entryZoneComponent.IsToaster
            //     ? toastZoneController.OnCollideToaster
            //     : toastZoneController.OnCollideBadThing);
            entryZoneComponent.SetZoneEnteredEvent(zoneCallback);

            var zoneExitCallback = new UnityGameObjectEvent();
            if (entryZoneComponent.IsToaster) zoneExitCallback.AddListener(toastZoneController.OnExitFromCollider);
            entryZoneComponent.SetZoneExitEvent(zoneExitCallback);

            // var coinZoneEneteredCallback = new UnityGameObjectEvent();
            // if (entryZoneComponent.gameObject.tag == "Coin") coinZoneEneteredCallback.AddListener(toastZoneController.OnCollideCoin);
        }
    }
}
