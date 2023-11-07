using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace BreadFlip.Sound
{
    public class VolumeChanging : MonoBehaviour
    {
        [SerializeField]
        AudioMixerGroup mixer;

        [SerializeField]
        Toggle musicToggle;

        [SerializeField]
        bool BackgroundMusic;

        private void OnEnable()
        {
            string s;
            if (BackgroundMusic) s = "BackVolume";
            else s = "EffectsVolume";
                        
            if (!PlayerPrefs.HasKey("Current" + s)) return;

            if (PlayerPrefs.GetInt("Current" + s) == 1)
            {
                mixer.audioMixer.SetFloat(s, 0f);
                musicToggle.isOn = true;
            }
            else
            {
                mixer.audioMixer.SetFloat(s, -80f);
                musicToggle.isOn = false;
            }
        }

        public void ChangeVolume()
        {
            string s;
            if (BackgroundMusic) s = "BackVolume";
            else s = "EffectsVolume";

            if (musicToggle.isOn)
            {
                mixer.audioMixer.SetFloat(s, 0f);
                PlayerPrefs.SetInt("Current" + s, 1);
            }
            else
            {
                mixer.audioMixer.SetFloat(s, -80f);
                PlayerPrefs.SetInt("Current" + s, 0);
            }
        }
    }
}
