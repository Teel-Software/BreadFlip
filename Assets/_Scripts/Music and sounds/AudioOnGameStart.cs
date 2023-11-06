using UnityEngine;
using UnityEngine.Audio;

namespace BreadFlip.Sound
{
    public class AudioOnGameStart : MonoBehaviour
    {

        [SerializeField] AudioMixerGroup backgroundMixer;
        [SerializeField] AudioMixerGroup effectsMixer;

        void Awake()
        {
            if (PlayerPrefs.HasKey("CurrentBackVolume"))
                if (PlayerPrefs.GetInt("CurrentBackVolume") == 1)
                    backgroundMixer.audioMixer.SetFloat("BackVolume", 0f);
                else
                    backgroundMixer.audioMixer.SetFloat("BackVolume", -80f);

            if (PlayerPrefs.HasKey("CurrentEffectsVolume"))
                if (PlayerPrefs.GetInt("CurrentEffectsVolume") == 1)
                    backgroundMixer.audioMixer.SetFloat("EffectsVolume", 0f);
                else
                    backgroundMixer.audioMixer.SetFloat("EffectsVolume", -80f);
        }
    }
}
