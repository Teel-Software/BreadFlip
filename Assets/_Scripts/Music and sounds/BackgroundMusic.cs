using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadFlip.Sound
{
    public class BackgroundMusic : MonoBehaviour
    {
        public static BackgroundMusic instance;
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(transform.root.gameObject);
                GetComponent<AudioSource>().Play();
            }
            else
            {
                if (instance != this)
                {
                    Destroy(transform.root.gameObject, 0.2f);
                }
            }
        }

            #region Вариант 1
            // private static BackgroundMusic backgroundMusic;
            // private void Awake()
            // {
            //     DontDestroyOnLoad(gameObject);
            //     if (backgroundMusic == null)
            //     {
            //         backgroundMusic = this;
            //     }
            //     else
            //     {
            //         Destroy(gameObject);
            //     }
            // }
            #endregion

            #region Вариант 2
            // private AudioSource _audioSource;

            // private void Awake() {
            //     DontDestroyOnLoad(gameObject);
            //     _audioSource = GetComponent<AudioSource>();
            //     PlayMusic();
            // }

            // private void PlayMusic()
            // {
            //     if (_audioSource.isPlaying) return;
            //     _audioSource.Play();
            // }
            #endregion
        }
}
