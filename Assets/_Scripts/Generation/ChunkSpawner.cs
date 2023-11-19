using System;
using System.Collections.Generic;
using System.Linq;
using BreadFlip.Movement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BreadFlip.Generation
{
    public class ChunkSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnParent;

        [SerializeField] private List<Chunk> _chunkPrefabs = new();

        [SerializeField] private Chunk[] _defaultSpawnedChunks;

        [SerializeField] private float _offsetDistanceToLastChunkX = 15f;
        [SerializeField] private float _offsetDistanceToFirstChunkX = 5f;
        [SerializeField] private int _minSpawnedChunks = 5;
        [SerializeField] private int _maxSpawnedChunks = 10;

        [Space, SerializeField] private Chunk _emptySpaceVariant;
        [SerializeField] private float _emptySpaceValue = 3f;

        private ToastZoneController _player;
        private readonly List<Chunk> _spawnedChunks = new();

        private void OnValidate()
        {
            _spawnParent ??= transform;
            if (_defaultSpawnedChunks == null || _defaultSpawnedChunks.Length == 0)
            {
                _defaultSpawnedChunks = new Chunk[transform.childCount];
                for (var i = 0; i < transform.childCount; i++)
                {
                    _defaultSpawnedChunks[i] = transform.GetChild(i).GetComponent<Chunk>();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!_player) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_player.transform.position - _offsetDistanceToLastChunkX * Vector3.right, 1f);
            Gizmos.DrawSphere(_player.transform.position + _offsetDistanceToFirstChunkX * Vector3.right, 1f);
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_spawnedChunks.Last().EndSpawnPoint.transform.position - _offsetDistanceToFirstChunkX * Vector3.right, 1f);
            Gizmos.DrawSphere(_spawnedChunks.First().StartSpawnPoint.transform.position + _offsetDistanceToLastChunkX * Vector3.right, 1f);
        }

        private void Start()
        {
            var player = FindObjectOfType<ToastZoneController>();
            
            _player = player;
            if (_emptySpaceVariant) InitEmptySpaceVariant();
                
            InitStartTables();
            Spawn();
        }

        private void InitEmptySpaceVariant()
        {
            _emptySpaceVariant.StartSpawnPoint.transform.localPosition = new Vector3(_emptySpaceValue / 2, 0, 0);
            _emptySpaceVariant.EndSpawnPoint.transform.localPosition = new Vector3(-_emptySpaceValue / 2, 0, 0);

            _chunkPrefabs.Add(_emptySpaceVariant);
        }

        private void Update()
        {
            if (!_player) return;
            
            var lastTableEndPosition = _spawnedChunks.Last().EndSpawnPoint.transform.position;
            var firstTableStartPosition = _spawnedChunks.First().StartSpawnPoint.transform.position;

            if (_player.transform.position.x < lastTableEndPosition.x + _offsetDistanceToLastChunkX)
            {
                Spawn();
            }

            if (_spawnedChunks.Count > _maxSpawnedChunks ||
                (_spawnedChunks.Count > _minSpawnedChunks &&
                 _player.transform.position.x < firstTableStartPosition.x - _offsetDistanceToFirstChunkX))
                DestroyTable();
        }

        private void DestroyTable()
        {
            Destroy(_spawnedChunks[0].gameObject);
            _spawnedChunks.RemoveAt(0);
        }

        [ContextMenu("Spawn")]
        private void Spawn()
        {
            var newChunk = Instantiate(_chunkPrefabs[Random.Range(0, _chunkPrefabs.Count)] /*GetRandomTableVariant()*/,
                _spawnParent);

            newChunk.ReplacePrefabInChunk();
            newChunk.RegisterEntryZones(_player);
            
            UpdatePosition(newChunk, _spawnedChunks.Last());
            _spawnedChunks.Add(newChunk);
        }

        private void UpdatePosition(Chunk newChunk, Chunk lastChunk)
        {
            var lastTableEndPosition = lastChunk.EndSpawnPoint.transform.position;
            var newTableStartPosition = newChunk.StartSpawnPoint.transform.localPosition;

            newChunk.transform.position = lastTableEndPosition - newChunk.transform.localScale.x * newTableStartPosition;
        }

        private Chunk GetRandomTableVariant()
        {
            var chances = new List<float>();

            foreach (var tableVariant in _chunkPrefabs)
            {
                var chance = tableVariant.ChanceFromDistance.Evaluate(-_player.transform.position.x);
                chances.Add(chance);
            }

            var randomChance = Random.Range(0, chances.Sum());
            var currentChance = 0f;

            for (int i = 0; i < chances.Count; i++)
            {
                currentChance += chances[i];

                if (randomChance <= currentChance)
                {
                    return _chunkPrefabs[i];
                }
            }

            return _chunkPrefabs[_chunkPrefabs.Count - 1];
        }

        private void InitStartTables()
        {
            for (var i = 0; i < _defaultSpawnedChunks.Length; i++)
            {
                var spawnedTable = _defaultSpawnedChunks[i];
                _spawnedChunks.Add(spawnedTable);
                spawnedTable.ReplacePrefabInChunk();
                spawnedTable.RegisterEntryZones(_player);

                if (i > 0)
                {
                    var prevChunk = _defaultSpawnedChunks[i - 1];
                    UpdatePosition(spawnedTable, prevChunk);
                }
            }
        }
    }
}