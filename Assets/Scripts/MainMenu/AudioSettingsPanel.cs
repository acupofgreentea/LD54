using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsPanel : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer sfxMixer;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(HandleMusicSliderValueChanged);
        sfxSlider.onValueChanged.AddListener(HandleSfxSliderValueChanged);
    }

    private void HandleSfxSliderValueChanged(float value)
    {
        DataManager.SfxVolume = value;
        sfxMixer.SetFloat("SfxVolume", Mathf.Log10(value) * 20);
    }

    private void HandleMusicSliderValueChanged(float value)
    {
        DataManager.MusicVolume = value;
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    private void OnEnable()
    {
        musicSlider.value = DataManager.MusicVolume;
        sfxSlider.value = DataManager.SfxVolume;
    }
}
