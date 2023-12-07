using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadFlip.Sound
{
    public class SoundManager : MonoBehaviour
    {
        //[SerializeField] private AudioClip jumpFirst;
        [SerializeField] private AudioClip jumpSecond;
        [SerializeField] private AudioClip failed;
        [SerializeField] private AudioClip doubleJump;
        [SerializeField] private AudioClip landedInToaster;
        [SerializeField] private AudioClip rewardSound;

        private AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        public void PlayJumpSecond() => source.PlayOneShot(jumpSecond);
        public void PlayFailedSound() => source.PlayOneShot(failed);
        public void PlayDoubleJump () => source.PlayOneShot(doubleJump); 
        public void StopDoubleJumpSound() => source.Stop();
        public void PlayLandedInToasterSound() => source.PlayOneShot(landedInToaster);
        public void PlayRewardSound() => source.PlayOneShot(rewardSound);
    }
}
