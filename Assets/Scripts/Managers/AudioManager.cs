using UnityEngine.Audio;
using UnityEngine;


public class AudioManager: MonoBehaviour
{
    [Space(5)]
    [SerializeField] private AudioMixer mixer;

    private void Start()
    {
        LoadVolume();
    }

    public void SetVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mixer.SetFloat("MasterVolume", volume);

        PlayerPrefs.SetFloat("MasterVolumeSlider", value);
    }

    public void SetMusicVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mixer.SetFloat("MusicVolume", volume);

        PlayerPrefs.SetFloat("MusicVolumeSlider", value);
    }

    public void SetSFXVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mixer.SetFloat("SFXVolume", volume);

        PlayerPrefs.SetFloat("SFXVolumeSlider", value);
    }

    public void LoadVolume()
    {
        float master = PlayerPrefs.GetFloat("MasterVolumeSlider", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolumeSlider", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolumeSlider", 1f);

        mixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(master, 0.0001f, 1f)) * 20f);
        mixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(music, 0.0001f, 1f)) * 20f);
        mixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(sfx, 0.0001f, 1f)) * 20f);
    }
}

