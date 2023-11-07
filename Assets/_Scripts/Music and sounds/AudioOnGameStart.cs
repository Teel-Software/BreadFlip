using UnityEngine;
using UnityEngine.Audio;

namespace BreadFlip.Sound
{
    public class AudioOnGameStart : MonoBehaviour
    {
        [SerializeField] AudioMixer mixer;

        void Start()
        {
            if (PlayerPrefs.HasKey("CurrentBackVolume"))
                if (PlayerPrefs.GetInt("CurrentBackVolume") == 1)
                    mixer.SetFloat("BackVolume", 0f);
                else
                    mixer.SetFloat("BackVolume", -80f);

            if (PlayerPrefs.HasKey("CurrentEffectsVolume"))
                if (PlayerPrefs.GetInt("CurrentEffectsVolume") == 1)
                    mixer.SetFloat("EffectsVolume", 0f);
                else
                    mixer.SetFloat("EffectsVolume", -80f);
        }
    }
}
