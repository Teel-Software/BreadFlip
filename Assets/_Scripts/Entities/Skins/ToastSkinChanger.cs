using System;
using UnityEngine;
using BreadFlip.UI;

namespace BreadFlip.Entities.Skins
{
    public class ToastSkinChanger : MonoBehaviour
    {
        public Toast CurrentSkin { get; private set; }

        [SerializeField] private Toast[] skins;
        [SerializeField] private AbstractSkinNeedy[] _skinNeedies;

        [SerializeField] private Toast _defaultSkin;

        private void Awake()
        {
            // скин, который ставится по дефолту, при старте игры
            // CurrentSkin = _defaultSkin;
            // ChangeSkin(CurrentSkin);
            if (PlayerPrefs.HasKey("SKIN_EQUPPIED"))
                ChangeSkin(PlayerPrefs.GetInt("SKIN_EQUPPIED"));
            else
            {
                ChangeSkin(_defaultSkin);
            }
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
            var index = Array.IndexOf(skins, CurrentSkin);
            var nextIndex = (index + 1) % skins.Length;

            var nextSkin = skins[nextIndex];
            ChangeSkin(nextSkin);
        }

        public void PreviousSkin()
        {
            var index = Array.IndexOf(skins, CurrentSkin);
            var previousIndex = index - 1;
            if (index < 0)
            {
                previousIndex = skins.Length - 1;
            }

            var previousSkin = skins[previousIndex];
            ChangeSkin(previousSkin);
        }

        // основная функция. нужно передать тост из списка _skins, чтобы включить
        public void ChangeSkin(Toast newSkin)
        {
            CurrentSkin = newSkin;
            foreach (var skin in skins)
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
            if (newSkinIndex < 0 || newSkinIndex >= skins.Length)
                return;

            CurrentSkin = skins[newSkinIndex];
            
            foreach (var skin in skins)
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