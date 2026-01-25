using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] public AudioSource sfxSource;
    [SerializeField] public AudioSource uiSource;
    [SerializeField] public AudioSource musicSource;

    [Header("Music Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip inMatchMusic;

    [Header("SFX Clips")]
    public AudioClip turretShoot;
    public AudioClip enemiesHit;
    public AudioClip trapExplode;

    [Header("UI Clips")]
    public AudioClip uiClick;
    public AudioClip unitSlotClick;
    public AudioClip unitPlacedRemoved;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadVolume();
    }

    public void PlayUIClick()
    {
        PlayUI(uiClick, 0.16f);
    }
    public void PlayUnitSelectClick()
    {
        PlayUI(unitSlotClick, 0.48f);
    }
    public void PlayUnitPlacedRemoved()
    {
        PlayUI(unitPlacedRemoved, 0.6f);
    }

    private void PlayUI(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null) return;
        uiSource.PlayOneShot(clip, volumeScale);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        if (SoundLimiter.Instance.CanPlay(clip, 3, 1f) == false)
        {
            return;
        }
        sfxSource.PlayOneShot(clip);
    }

    public void PlayTurretShootSound()
    {
        PlaySFX(turretShoot);
    }
    public void PlayEnemiesHitSound()
    {
        PlaySFX(enemiesHit);
    }
    public void PlayTrapExplodeSound()
    {
        PlaySFX(trapExplode);
    }

    public void PlayMusic(bool mainMenu = false, bool loop = true)
    {
        AudioClip temp = mainMenu ? mainMenuMusic : inMatchMusic;
        if (musicSource.clip == temp && musicSource.isPlaying)
            return;
        musicSource.clip = temp;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    private void LoadVolume()
    {
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        uiSource.volume = PlayerPrefs.GetFloat("UIVolume", 0.5f);
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        PlayMusic(true);
    }
}
