using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace BreadFlip
{
    public class ChangeWallAndFloorMaterial : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _wallTopPrefab;
        [SerializeField] private GameObject _wallBottomPrefab;
        [SerializeField] private GameObject _floorPrefab;

        [Header("Skins")]
        [SerializeField] private List<Material> walls;
        [SerializeField] private List<Material> floors;

        [Header("Default materials")]
        [SerializeField] private Material _defaultWall;
        [SerializeField] private Material _defaultFloor;

        private Dictionary<int, Material[]> dict = new Dictionary<int, Material[]>();

        // скины будут нумероваться ( 0 - дефолт, 1 - первый и т.д.)
        // внутри словаря первый элемент значения - это всегда стена, второй - пол
        private void Awake() 
        {
            SetDict();
            if (PlayerPrefs.HasKey("current_kitchen"))
            {
                SetWallAndFloor(
                    dict[PlayerPrefs.GetInt("current_kitchen")][0],
                    dict[PlayerPrefs.GetInt("current_kitchen")][1]
                );
            }
            else
            {
                // SetWallAndFloor(_defaultWall, _defaultFloor);
                SetWallAndFloor(walls[0], floors[0]);
            }
        }

        private void SetDict()
        {
            for (int i = 0; i < walls.Count; i ++)
            {
                dict.Add(i, new Material[] { walls[i], floors[i] });
            }
        }

        public void SetWallAndFloor(Material wall, Material floor)
        {
            Debug.Log($"ChunkElement: {_wallTopPrefab.transform.Find("ChunkElement")}");
            _wallTopPrefab.transform.Find("ChunkElement").gameObject.GetComponent<MeshRenderer>().material = wall;
            _wallBottomPrefab.transform.Find("ChunkElement").gameObject.GetComponent<MeshRenderer>().material = wall;

            _floorPrefab.transform.GetComponent<MeshRenderer>().material = floor;
        }
    }
}
