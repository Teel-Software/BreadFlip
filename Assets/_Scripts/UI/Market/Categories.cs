using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BreadFlip.UI
{
    public class Categories : MonoBehaviour
    {
        public GameObject breadCategoryPrefab;
        public GameObject ToastsCategoryPrefab;
        public GameObject KitchenCategoryPrefab;

        public List<GameObject> _breadCellsPrefabs;
        public List<GameObject> _toastsCellsPrefabs;
        public List<GameObject> _kitchenCellsPrefabs;
    }
}
