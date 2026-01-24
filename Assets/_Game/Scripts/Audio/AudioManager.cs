using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Manages audio volume settings for the game using Unity's AudioMixer system.
/// Handles master, music, and SFX volume channels with automatic persistence via PlayerPrefs.
/// Can optionally be connected to UI sliders for a settings menu.
/// </summary>
/// <remarks>
/// <para>
/// This component requires an AudioMixer asset with exposed parameters named:
/// "MasterVolume", "MusicVolume", and "SFXVolume".
/// </para>
/// <para>
/// Volume values are expected in the range [0, 1] (linear scale) and are automatically
/// converted to decibels for the AudioMixer using the formula: dB = 20 * log10(value).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Set music volume to 50%
/// audioManager.SetMusicVolume(0.5f);
///
/// // Mute SFX
/// audioManager.SetSFXVolume(0f);
/// </code>
/// </example>
public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Volume Sliders (Optional - for settings menu)")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string MASTER_VOLUME = "MasterVolume";
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";

    void Start()
    {
        LoadVolumes();

        if (masterSlider != null)
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    /// <summary>
    /// Sets the master volume level and persists it to PlayerPrefs.
    /// </summary>
    /// <param name="volume">Volume level from 0 (muted) to 1 (full volume).</param>
    public void SetMasterVolume(float volume)
    {
        SetVolume(MASTER_VOLUME, volume);
        PlayerPrefs.SetFloat(MASTER_VOLUME, volume);
    }

    /// <summary>
    /// Sets the music volume level and persists it to PlayerPrefs.
    /// </summary>
    /// <param name="volume">Volume level from 0 (muted) to 1 (full volume).</param>
    public void SetMusicVolume(float volume)
    {
        SetVolume(MUSIC_VOLUME, volume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
    }

    /// <summary>
    /// Sets the sound effects volume level and persists it to PlayerPrefs.
    /// </summary>
    /// <param name="volume">Volume level from 0 (muted) to 1 (full volume).</param>
    public void SetSFXVolume(float volume)
    {
        SetVolume(SFX_VOLUME, volume);
        PlayerPrefs.SetFloat(SFX_VOLUME, volume);
    }

    private void SetVolume(string parameterName, float sliderValue)
    {
        float dB = sliderValue > 0.0001f ? 20f * Mathf.Log10(sliderValue) : -80f;
        mixer.SetFloat(parameterName, dB);
    }

    private void LoadVolumes()
    {
        float masterVol = PlayerPrefs.GetFloat(MASTER_VOLUME, 1f);
        float musicVol = PlayerPrefs.GetFloat(MUSIC_VOLUME, 1f);
        float sfxVol = PlayerPrefs.GetFloat(SFX_VOLUME, 1f);

        SetMasterVolume(masterVol);
        SetMusicVolume(musicVol);
        SetSFXVolume(sfxVol);

        if (masterSlider != null) masterSlider.value = masterVol;
        if (musicSlider != null) musicSlider.value = musicVol;
        if (sfxSlider != null) sfxSlider.value = sfxVol;
    }
}