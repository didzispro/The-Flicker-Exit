using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Space(5)]
    [Header("Sliders")]
    [Space(5)]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [Space(10)]

    public AudioManager audiomanager;

    void Start()
    {
        // LOAD saved values
        float master = PlayerPrefs.GetFloat("MasterVolumeSlider", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolumeSlider", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolumeSlider", 1f);

        // SET sliders WITHOUT triggering events
        masterSlider.SetValueWithoutNotify(master);
        musicSlider.SetValueWithoutNotify(music);
        sfxSlider.SetValueWithoutNotify(sfx);

        // APPLY to audio
        audiomanager.SetVolume(master);
        audiomanager.SetMusicVolume(music);
        audiomanager.SetSFXVolume(sfx);
    }
}
