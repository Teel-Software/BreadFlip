using System;
using System.Collections.Generic;
using System.Linq;
using BreadFlip.Movement;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BreadFlip.Generation
{
    public class ChunkSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnParent;

        [SerializeField] private List<Chunk> _chunkPrefabs = new();
        [SerializeField] private List<PropsElement> _propsElements;

        [SerializeField] private Chunk[] _defaultSpawnedChunks;

        [SerializeField] private float _offsetDistanceToLastChunkX = 15f;
        [SerializeField] private float _offsetDistanceToFirstChunkX = 5f;
        [SerializeField] private int _minSpawnedChunks = 5;
        [SerializeField] private int _maxSpawnedChunks = 10;

        [Space, SerializeField] private Chunk _emptySpaceVariant;
        [SerializeField] private float _emptySpaceValue = 2f;

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
                
            InitStartChunks();
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
                DestroyChunk();
        }

        private void DestroyChunk()
        {
            Destroy(_spawnedChunks[0].gameObject);
            _spawnedChunks.RemoveAt(0);
        }

        private void Spawn()
        {
            var newChunk = Instantiate(_chunkPrefabs[Random.Range(0, _chunkPrefabs.Count)] /*GetRandomChunkVariant()*/,
                _spawnParent);

            UpdateSpawnedChunk(newChunk, _spawnedChunks[^1]);
        }

        private void UpdatePosition(Chunk newChunk, Chunk lastChunk)
        {
            var lastTableEndPosition = lastChunk.EndSpawnPoint.transform.position;
            var newTableStartPosition = newChunk.StartSpawnPoint.transform.localPosition;

            newChunk.transform.position = lastTableEndPosition - newChunk.transform.localScale.x * newTableStartPosition;
        }

        private Chunk GetRandomChunkVariant()
        {
            var chances = new List<float>();

            foreach (var chunkVariant in _chunkPrefabs)
            {
                var chance = chunkVariant.ChanceFromDistance.Evaluate(-_player.transform.position.x);
                chances.Add(chance);
            }

            var randomChance = Random.Range(0, chances.Sum());
            var currentChance = 0f;

            for (var i = 0; i < chances.Count; i++)
            {
                currentChance += chances[i];

                if (randomChance <= currentChance)
                {
                    return _chunkPrefabs[i];
                }
            }

            return _chunkPrefabs[^1];
        }

        private void InitStartChunks()
        {
            for (var i = 0; i < _defaultSpawnedChunks.Length; i++)
            {
                var spawnedChunk = _defaultSpawnedChunks[i];

                var previousChunk = i > 0 ? _defaultSpawnedChunks[i - 1] : null;
                UpdateSpawnedChunk(spawnedChunk, previousChunk);
            }
        }

        private void UpdateSpawnedChunk(Chunk spawnedChunk,  [CanBeNull] Chunk previousChunk)
        {
            _spawnedChunks.Add(spawnedChunk);
            spawnedChunk.ReplacePrefabInChunk();
            spawnedChunk.SpawnProps(_propsElements);
            spawnedChunk.RegisterEntryZones(_player);

            if (previousChunk)
            {
                UpdatePosition(spawnedChunk, previousChunk);
            }
        }
    }
}