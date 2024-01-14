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
            // скин, который ставится по дефолту, при старте игры
            CurrentSkin = _defaultSkin;
            ChangeSkin(CurrentSkin);
        }

        private void Update()
        {
            // переключение скинов. по сути только для демонстриации
            if (Input.GetKeyDown(KeyCode.R))
            {
                NextSkin();
            }
        }

        public void NextSkin()
        {
            var index = Array.IndexOf(_skins, CurrentSkin);
            var nextIndex = (index + 1) % _skins.Length;

            var nextSkin = _skins[nextIndex];
            ChangeSkin(nextSkin);
        }

        public void PreviousSkin()
        {
            var index = Array.IndexOf(_skins, CurrentSkin);
            var previousIndex = index - 1;
            if (index < 0)
            {
                previousIndex = _skins.Length - 1;
            }

            var previousSkin = _skins[previousIndex];
            ChangeSkin(previousSkin);
        }

        // основная функция. нужно передать тост из списка _skins, чтобы включить
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
        
        public void ChangeSkin(int newSkinIndex)
        {
            if (newSkinIndex <= 0 || newSkinIndex >= _skins.Length)
                return;

            CurrentSkin = _skins[newSkinIndex];
            
            foreach (var skin in _skins)
            {
                skin.gameObject.SetActive(skin == CurrentSkin);
            }

            foreach (var skinNeedy in _skinNeedies)
            {
                skinNeedy.UpdateSkin(CurrentSkin);
            }
        }
    }
}