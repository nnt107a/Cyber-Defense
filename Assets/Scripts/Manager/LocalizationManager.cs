using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour
{
    private void Awake()
    {
        int localeIndex = PlayerPrefs.GetInt("LocaleIndex", 0);
        StartCoroutine(SetLocale(localeIndex));
    }
    private IEnumerator SetLocale(int index)
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
    }
}
