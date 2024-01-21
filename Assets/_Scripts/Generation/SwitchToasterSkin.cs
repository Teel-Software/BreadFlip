using UnityEngine;

namespace BreadFlip.Generation
{
    public class SwitchToasterSkin : MonoBehaviour
    {
        [SerializeField] private GameObject _toasterSkins;
        [SerializeField] private ChunkSpawner _chunkSpawner;

        private int skinsCount;
        // private Dictionary<int, GameObject> dict = new Dictionary<int, GameObject>();

        private void OnEnable()
        {
            skinsCount = _toasterSkins.GetComponent<ToasterSkins>().skinsPrefabs.Count;
            
        }

        public void SetToasterSkin(int skinIndex)
        {
            Debug.Log(skinsCount);
            for (int i = 0; i < skinsCount; i++)
            {
                if (i == skinIndex)
                {
                    _toasterSkins.GetComponent<ToasterSkins>().skinsPrefabs[i].SetActive(true);
                    Debug.Log("I switched");
                }
                else
                {
                    _toasterSkins.GetComponent<ToasterSkins>().skinsPrefabs[i].SetActive(false);
                    Debug.Log("I did not switch");
                }
            }
            _chunkSpawner.UpdateChunks(skinIndex);
        }
    }
}
