using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSource;
    [SerializeField] private AudioSource musicSource;

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
    }

    public void PlayUIClick()
    {
        PlayUI(uiClick, 0.2f);
    }
    public void PlayUnitSelectClick()
    {
        PlayUI(unitSlotClick, 1.2f);
    }
    public void PlayUnitPlacedRemoved()
    {
        PlayUI(unitPlacedRemoved, 1.5f);
    }

    private void PlayUI(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null) return;
        uiSource.PlayOneShot(clip, volumeScale);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
