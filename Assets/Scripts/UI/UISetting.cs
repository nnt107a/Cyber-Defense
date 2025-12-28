using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    private void Start()
    {
        GameManager.Instance.OnGoToSetting += Show;
        musicVolumeSlider.value = SoundManager.Instance.musicSource.volume;
        sfxVolumeSlider.value = SoundManager.Instance.sfxSource.volume;

        musicVolumeSlider.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance.musicSource.volume = value;
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save();
        });
        sfxVolumeSlider.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance.sfxSource.volume = value;
            SoundManager.Instance.uiSource.volume = value;
            PlayerPrefs.SetFloat("SFXVolume", value);
            PlayerPrefs.SetFloat("UIVolume", value);
            PlayerPrefs.Save();
        });
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnPause -= Show;
    }
    private void Show()
    {
        panel.interactable = true;
        panel.blocksRaycasts = true;
        panel.DOFade(1f, 0.5f).SetUpdate(true);
    }
    public void Hide()
    {
        panel.DOFade(0f, 0.5f).SetUpdate(true).OnComplete(() =>
        {
            panel.interactable = false;
            panel.blocksRaycasts = false;
        });
    }
    public void SetLanguage(int localeIndex)
    {
        StartCoroutine(SetLocale(localeIndex));
    }

    IEnumerator SetLocale(int index)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[index];

        yield return null;

        LocalizeStringEvent[] allLocalizeComponents = FindObjectsByType<LocalizeStringEvent>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var localizeComponent in allLocalizeComponents)
        {
            localizeComponent.RefreshString();
        }
        Debug.Log("Language set to: " + LocalizationSettings.SelectedLocale.LocaleName);
        PlayerPrefs.SetInt("LocaleIndex", index);
        PlayerPrefs.Save();
    }
}
