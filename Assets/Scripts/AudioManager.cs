using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundTrackSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Track Audio Clips")]
    [SerializeField] private AudioClip menuTrack;
    [SerializeField] private AudioClip gameTrack;

    [Header("SFX Audio Clips")]
    [SerializeField] private AudioClip boostSfxClip;
    [SerializeField] private AudioClip burningSfxClip;
    [SerializeField] private AudioClip errorSfxClip;
    [SerializeField] private AudioClip fallingSfxClip;
    [SerializeField] private AudioClip obstacleHitSfxClip;
    [SerializeField] private AudioClip stickySfxClip;
    [SerializeField] private AudioClip suppliesSfxClip;

    private const string MUTE_KEY = "MuteSetting";

    private bool isMuted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey(MUTE_KEY))
        {
            isMuted = PlayerPrefs.GetInt(MUTE_KEY) == 1;
            UpdateAudioSources(isMuted);
        }
    }

    public void MuteSounds(bool mute)
    {
        isMuted = mute;
        UpdateAudioSources(mute);

        PlayerPrefs.SetInt(MUTE_KEY, isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void UpdateAudioSources(bool mute)
    {
        backgroundTrackSource.mute = mute;
        sfxSource.mute = mute;
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public void PlayMenuTrack()
    {
        backgroundTrackSource.clip = menuTrack;
        backgroundTrackSource.Play();
    }

    public void PlayGameTrack()
    {
        backgroundTrackSource.clip = gameTrack;
        backgroundTrackSource.Play();
    }

    public void PlayBoostSfx()
    {
        sfxSource.PlayOneShot(boostSfxClip);
    }

    public void PlayBurningSfx()
    {
        sfxSource.PlayOneShot(burningSfxClip);
    }

    public void PlayErrorSfx()
    {
        sfxSource.PlayOneShot(errorSfxClip);
    }

    public void PlayFallingSfx()
    {
        sfxSource.PlayOneShot(fallingSfxClip);
    }

    public void PlayObstacleHitSfx()
    {
        sfxSource.PlayOneShot(obstacleHitSfxClip);
    }

    public void PlayStickySfx()
    {
        sfxSource.PlayOneShot(stickySfxClip);
    }

    public void PlaySuppliesSfx()
    {
        sfxSource.PlayOneShot(suppliesSfxClip);
    }
}
