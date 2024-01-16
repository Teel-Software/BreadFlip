using System.Collections.Generic;
using UnityEngine;

namespace BreadFlip
{
    public class BuyingCells : MonoBehaviour
    {
        private const int skinsCount = 6;

        [SerializeField] private List<GameObject> _cellsPrefabs;
        [SerializeField] private GameObject _blockedPrefab;

        private List<GameObject> _spawnedItems = new List<GameObject>();

        private void OnEnable() {
            ShowSkins();
        }

        private void ShowSkins()
        {
            ClearCells();
            for (int i = 1; i <= skinsCount; i ++)
            {
                if (i <= _cellsPrefabs.Count)
                {
                    InitItem(_cellsPrefabs[i-1]);
                }
                else
                {
                    InitItem(_blockedPrefab);
                }
            }
        }

        private void InitItem (GameObject prefab)
        {
            var spawnedItem = Instantiate(prefab, gameObject.transform);
            _spawnedItems.Add(spawnedItem);
        }

        private void ClearCells()
        {
            foreach (var el in _spawnedItems)
            {
                Destroy(el);
            }
            _spawnedItems.Clear();
        }
    }
}
