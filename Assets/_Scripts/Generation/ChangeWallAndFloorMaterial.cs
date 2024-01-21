using System.Collections.Generic;
using UnityEngine;

namespace BreadFlip
{
    public class ChangeWallAndFloorMaterial : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _wallTopPrefab;
        [SerializeField] private GameObject _wallBottomPrefab;
        [SerializeField] private GameObject _floorPrefab;

        [Header("Skins")]
        [SerializeField] private List<Texture> walls;
        [SerializeField] private List<Texture> floors;

        // [Header("Default materials")]
        // [SerializeField] private Texture _defaultWall;
        // [SerializeField] private Texture _defaultFloor;

        private Dictionary<int, Texture[]> dict = new Dictionary<int, Texture[]>();

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
                dict.Add(i, new Texture[] { walls[i], floors[i] });
            }
        }

        public void SetWallAndFloor(Texture wall, Texture floor)
        {
            _wallTopPrefab.transform.Find("ChunkElement").gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTex", wall);
            _wallBottomPrefab.transform.Find("ChunkElement").gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTex", wall);

            _floorPrefab.transform.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTex", floor);
        }
    }
}
