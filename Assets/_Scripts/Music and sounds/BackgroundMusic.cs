using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadFlip.Sound
{
    public class BackgroundMusic : MonoBehaviour
    {

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
