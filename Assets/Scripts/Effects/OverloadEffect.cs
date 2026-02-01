using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
public class OverloadEffect : MonoBehaviour, IPoolable
{
    [SerializeField]
    private EffectData overloadEffectData;
    private GameObject prefabRef;
    private float timer = 0f;

    [Header("Settings")]
    public float fadeInTime = 2f;
    public float blinkSpeed = 0.5f;

    [Header("Visual References")]
    public Volume postProcessVolume;

    public TextMeshProUGUI warningText;

    public void Init(GameObject prefab)
    {
        prefabRef = prefab;
        timer = -0.1f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > overloadEffectData.duration)
        {
            ObjectPool.Instance.Despawn(prefabRef, gameObject);
        }
    }

    public void OnDespawn()
    {
        StartCoroutine(FadeVolume(0.4f, 0, fadeInTime));
        RemoveEnvironmentalEffects();
    }

    public void OnSpawn()
    {
        if (postProcessVolume != null)
        {
            postProcessVolume.weight = 0;

            StartCoroutine(FadeVolume(0, 0.4f, fadeInTime));
        }
        if (warningText != null)
        {
            StartCoroutine(BlinkTextRoutine());
        }
        SoundManager.Instance.PlayOverloadWarningSound();
        ApplyEnvironmentalEffects();
    }

    private void ApplyEnvironmentalEffects()
    {
        var allTurrets = FindObjectsByType<TurretEffectController>(FindObjectsSortMode.None);
        foreach (var turret in allTurrets)
        {
            turret.ApplyEffect(overloadEffectData, turret.GetInstanceID());
            turret.gameObject.GetComponent<Turret>().UpdateStats();
            Debug.Log(
                "Overload Effect Zone applying slow effect to "
                    + turret.name
                    + " for "
                    + overloadEffectData.effectValue
                    + " slow effect"
            );
        }
    }

    private void RemoveEnvironmentalEffects()
    {
        var allTurrets = FindObjectsByType<TurretEffectController>(FindObjectsSortMode.None);
        foreach (var turret in allTurrets)
        {
            turret.ClearEffects();
            turret.gameObject.GetComponent<Turret>().UpdateStats();
            Debug.Log(
                "Overload Effect Zone removing slow effect from "
                    + turret.name
                    + " for "
                    + overloadEffectData.effectValue
                    + " slow effect"
            );
        }
    }

    private IEnumerator BlinkTextRoutine()
    {
        while (true)
        {
            warningText.alpha = 0;
            yield return new WaitForSeconds(blinkSpeed);

            warningText.alpha = 1;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }

    private IEnumerator FadeVolume(float start, float end, float time)
    {
        float elapsed = 0;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / time;
            if (postProcessVolume)
                postProcessVolume.weight = Mathf.Lerp(start, end, t);
            yield return null;
        }
        if (postProcessVolume)
            postProcessVolume.weight = end;
    }
}