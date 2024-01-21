using UnityEngine;

namespace BreadFlip.Generation
{
    public class SwitchToasterSkin : MonoBehaviour
    {
        [SerializeField] private ToasterSkins _toasterSkins;

        private int skinsCount;
        // private Dictionary<int, GameObject> dict = new Dictionary<int, GameObject>();

        private void OnEnable()
        {
            skinsCount = _toasterSkins.skinsPrefabs.Count;
        }

        public void SetToasterSkin(int skinIndex)
        {
            for (int i = 0; i < skinsCount; i++)
            {
                if (i == skinIndex)
                {
                    _toasterSkins.skinsPrefabs[i].SetActive(true);
                }
                else
                {
                    _toasterSkins.skinsPrefabs[i].SetActive(false);
                }
            }
        }
    }
}
