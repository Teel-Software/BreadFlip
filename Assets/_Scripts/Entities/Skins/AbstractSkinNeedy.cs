using UnityEngine;

namespace BreadFlip.Entities.Skins
{
    public abstract class AbstractSkinNeedy : MonoBehaviour
    {
        public ToastSkinChanger SkinChanger => _skinChanger;

        [SerializeField] private ToastSkinChanger _skinChanger;

        public virtual void UpdateSkin(Toast newSkin) { }
    }
}