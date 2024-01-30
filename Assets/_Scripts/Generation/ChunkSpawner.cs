using System;
using System.Collections.Generic;
using System.Linq;
using BreadFlip.Entities;
using BreadFlip.Generation.Props;
using BreadFlip.Movement;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BreadFlip.Generation
{
    public class ChunkSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnParent;

        [SerializeField] private List<Chunk> _chunkPrefabs = new();
        [SerializeField] private List<PropsElement> _propsElements;
        [SerializeField] private Coin _coinPrefab;

        [SerializeField] private Chunk[] _defaultSpawnedChunks;

        [SerializeField] private float _offsetDistanceToLastChunkX = 15f;
        [SerializeField] private float _offsetDistanceToFirstChunkX = 5f;
        [SerializeField] private int _minSpawnedChunks = 5;
        [SerializeField] private int _maxSpawnedChunks = 10;

        [Space, SerializeField] private Chunk _emptySpaceVariant;
        [SerializeField] private float _emptySpaceValue = 2f;
        [SerializeField]private int _maximumAllowedQuantityEmptyChunk = 3;

        private Vector3 END_OFFSET = new Vector3(1.75f, 2f, 0f);
        private const float HEIGHT = 1.15f;
        private const int COINS_AMOUNT = 5;

        // [SerializeField] private GameObject coinPrefab;

        // private readonly List<GameObject> _spawnedCoins = new();
        
        private ToastZoneController _player;
        private readonly List<Chunk> _spawnedChunks = new();
        private int _numberOfChunksInARow;

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
                UpdateChunks(PlayerPrefs.GetInt("TOASTER_EQUPPIED"));
            }

            if (_spawnedChunks.Count > _maxSpawnedChunks ||
                (_spawnedChunks.Count > _minSpawnedChunks &&
                 _player.transform.position.x < firstTableStartPosition.x - _offsetDistanceToFirstChunkX))
                DestroyChunk(0);
        }

        private void DestroyChunk(int index)
        {
            Destroy(_spawnedChunks[index].gameObject);
            _spawnedChunks.RemoveAt(index);

            // Destroy(_spawnedCoins[0].gameObject);
            // _spawnedCoins.RemoveAt(0);
        }

        public void UpdateChunks(int index)
        {
            // Debug.Log($"_spawnedChunks.Count: {_spawnedChunks.Count}");
            for(int i = 0; i < _spawnedChunks.Count; i++)
            {
                var toaster = _spawnedChunks[i].GetComponentInChildren<Toaster>();
                if (toaster != null) SwitchToaster(toaster, index);
            }
        }

        public void SwitchToaster(Toaster toaster, int skinIndex)
        {
            for (int i = 0; i < 6; i++)
            {
                if (i == skinIndex)
                {
                    // Debug.Log($"skinsPrefabs: {toaster.GetComponent<ToasterSkins>().skinsPrefabs.Count} || {skinIndex}");
                    toaster.GetComponent<ToasterSkins>().skinsPrefabs[i].SetActive(true);
                    // Debug.Log("I switched");
                }
                else
                {
                    toaster.GetComponent<ToasterSkins>().skinsPrefabs[i].SetActive(false);
                    // Debug.Log("I did not switch");
                }
            }
        }

        private void Spawn()
        {
            var chunkPrefab = _chunkPrefabs[Random.Range(0, _chunkPrefabs.Count)];

            if (chunkPrefab == _emptySpaceVariant)
            {
                _numberOfChunksInARow++;

                if (_numberOfChunksInARow > _maximumAllowedQuantityEmptyChunk)
                {
                    Debug.Log("Заспавненно больше пустых чанков чем можно, пересоздал");
                    var chunksWithoutEmpty = _chunkPrefabs.Where(chunk => chunk != _emptySpaceVariant).ToArray();
                    chunkPrefab = chunksWithoutEmpty[Random.Range(0, chunksWithoutEmpty.Length)];
                }
            }
            else
            {
                _numberOfChunksInARow = 0;
            }
            
            var newChunk = Instantiate(chunkPrefab /*GetRandomChunkVariant()*/,
                _spawnParent);

            UpdateSpawnedChunk(newChunk, _spawnedChunks[^1]);
        }

        public void SpawnCoins(Coin _coinPrefab, Chunk currentChunk, Chunk previousChunk)
        {
            if (currentChunk.FirstTable == null) return;
            Vector3 endPosition = currentChunk
            .FirstTable.EntryZoneComponents
            .First(zone => zone.TryGetComponent<Toaster>(out var _))
            .GetComponent<Toaster>().ToastPosition.position
            + END_OFFSET
            ;
            
            if (previousChunk.FirstTable == null) return;
            Vector3 startPosition = previousChunk
            .FirstTable.EntryZoneComponents
            .First(zone => zone.TryGetComponent<Toaster>(out var _))
            .GetComponent<Toaster>().ToastPosition.position
            // + offset
            ;

            int pointInPoints;

            var points = GetCoinPoints(startPosition, endPosition, HEIGHT);

            for (var i = 1; i <= COINS_AMOUNT; i++)
            {
                pointInPoints =  (points.Length / COINS_AMOUNT * i) - 1;

                var newCoin = Instantiate(_coinPrefab, points[pointInPoints], Quaternion.identity, currentChunk.transform);
                currentChunk.AddNewEntryZones(newCoin);
                // _spawnedCoins.Add(newCoin);
            }
        }

        public Vector3 ParabolaPoint(Vector3 start, Vector3 end, float height, float t)
        {
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

        private Vector3[] GetCoinPoints(Vector3 origin, Vector3 end, float height)
        {
            // var direction = -transform.forward;
            // Vector3 speed = new Vector3 (direction.x * 0.5f, 0.5f, direction.z * 0.5f);

            var points = new Vector3[100];

            for (var i = 0; i < points.Length; i++)
            {
                var t = i * 0.01f;

                points[i] = ParabolaPoint(origin, end, height, t);
            }
            return points;
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
            // SpawnDefaultCoins(_coinPrefab);
        }

        private void UpdateSpawnedChunk(Chunk spawnedChunk,  [CanBeNull] Chunk previousChunk)
        {
            _spawnedChunks.Add(spawnedChunk);
            spawnedChunk.ReplacePrefabInChunk();
            spawnedChunk.SpawnProps(_propsElements);

            if (previousChunk)
            {
                UpdatePosition(spawnedChunk, previousChunk);
                if (_coinPrefab != null) SpawnCoins(_coinPrefab, spawnedChunk, previousChunk);
            }

            spawnedChunk.RegisterEntryZones(_player);
        }
    }
}