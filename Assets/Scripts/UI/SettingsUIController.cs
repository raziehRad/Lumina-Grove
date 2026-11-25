
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;

public class SettingsUIController : MonoBehaviour
{

    [Header("Sliders")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Sensitivity Event")]
    [SerializeField] private UnityEvent<float> onSensitivityChanged;

    private const string SensitivityKey = "Sensitivity";
    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";

    private void Start()
    {
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        float sensitivity = PlayerPrefs.GetFloat(SensitivityKey, 75f);
        float music = PlayerPrefs.GetFloat(MusicKey, 1f);
        float sfx = PlayerPrefs.GetFloat(SFXKey, 1f);

        sensitivitySlider.SetValueWithoutNotify(sensitivity);
        musicSlider.SetValueWithoutNotify(music);
        sfxSlider.SetValueWithoutNotify(sfx);

        ApplySensitivity(sensitivity);
        AudioManager.Instance.SetMusicVolume(music);
        AudioManager.Instance.SetSFXVolume(sfx);
    }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat(SensitivityKey, value);
        ApplySensitivity(value);
    }

    private void ApplySensitivity(float value)
    {
        onSensitivityChanged?.Invoke(value);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }
    
    private void OnDestroy()
    {
        sensitivitySlider.onValueChanged.RemoveListener(SetSensitivity);
        musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }
}
