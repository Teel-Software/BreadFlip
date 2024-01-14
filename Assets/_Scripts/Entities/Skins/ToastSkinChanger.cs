using System;
using UnityEngine;

namespace BreadFlip.Entities.Skins
{
    public class ToastSkinChanger : MonoBehaviour
    {
        public Toast CurrentSkin { get; private set; }

        [SerializeField] private Toast[] _skins;
        [SerializeField] private AbstractSkinNeedy[] _skinNeedies;

        [SerializeField] private Toast _defaultSkin;

        private void Awake()
        {
            CurrentSkin = _defaultSkin;
            ChangeSkin(CurrentSkin);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                var index = Array.IndexOf(_skins, CurrentSkin);
                var nextIndex = (index + 1) % _skins.Length;

                var nextSkin = _skins[nextIndex];
                ChangeSkin(nextSkin);
            }
        }

        public void ChangeSkin(Toast newSkin)
        {
            CurrentSkin = newSkin;
            foreach (var skin in _skins)
            {
                skin.gameObject.SetActive(skin == newSkin);
            }

            foreach (var skinNeedy in _skinNeedies)
            {
                skinNeedy.UpdateSkin(newSkin);
            }
        }
    }
}