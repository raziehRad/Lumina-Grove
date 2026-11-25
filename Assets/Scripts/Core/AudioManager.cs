
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum SoundType
{
    CollectItem,
    DepositItem,
    LightTurnOn,
    TreeCompleted,
    ButtonClick,
    PanelOpen,
    birds
}

[Serializable]
public class SoundEntry
{
    public SoundType type;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";

    private const float MUTED_VOLUME = -80f;
    private const float NORMAL_VOLUME = 0f;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Sound Effects")]
    [SerializeField] private List<SoundEntry> sounds;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;
    private Dictionary<SoundType, AudioClip> soundDictionary;

    private bool musicEnabled = true;
    private bool sfxEnabled = true;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
  

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        soundDictionary = new Dictionary<SoundType, AudioClip>();

        foreach (SoundEntry sound in sounds)
        {
            if (sound.clip != null)
                soundDictionary[sound.type] = sound.clip;
        }

        musicSource.loop = true;
        musicSource.playOnAwake = false;

        sfxSource.playOnAwake = false;
        
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        ApplyAudioSettings();
    }

    private void ApplyAudioSettings()
    {
        ApplyMusicVolume();
        ApplySFXVolume();

        musicToggle.SetIsOnWithoutNotify(!musicEnabled);
        sfxToggle.SetIsOnWithoutNotify(!sfxEnabled);
    }
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

        ApplyMusicVolume();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();

        ApplySFXVolume();
    }

    private void ApplyMusicVolume()
    {
        float volume = musicEnabled ? VolumeToDB(musicVolume) : MUTED_VOLUME;
        audioMixer.SetFloat(MUSIC_VOLUME, volume);
    }

    private void ApplySFXVolume()
    {
        float volume = sfxEnabled ? VolumeToDB(sfxVolume) : MUTED_VOLUME;
        audioMixer.SetFloat(SFX_VOLUME, volume);
    }

    private float VolumeToDB(float value)
    {
        return value <= 0.0001f ? MUTED_VOLUME : Mathf.Log10(value) * 20f;
    }
    private void Start()
    {
        SetMusicEnabled(musicEnabled);
        SetSFXEnabled(sfxEnabled);
    }
    public void PlayMusic()
    {
        if (!musicEnabled || backgroundMusic == null)
            return;

        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void PlaySFX(SoundType type)
    {
        if (!sfxEnabled)
            return;

        if (!soundDictionary.TryGetValue(type, out AudioClip clip))
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void PlayButton()
    {
        if (!sfxEnabled)
            return; 
        PlaySFX(SoundType.ButtonClick);
    }
    public void SetMusicEnabled(bool value)
    {
        musicEnabled = value;

        ApplyMusicVolume();
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetSFXEnabled(bool value)
    {
        sfxEnabled = value;

        ApplySFXVolume();
        PlayerPrefs.SetInt("SFXEnabled", sfxEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnMusicToggleChanged(bool isOn)
    {
        SetMusicEnabled(!isOn);
    }

    public void OnSFXToggleChanged(bool isOn)
    {
        SetSFXEnabled(!isOn);
    }
}
