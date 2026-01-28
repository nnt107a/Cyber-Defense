using DG.Tweening.Core.Easing;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelStateBar : MonoBehaviour
{
    public static UILevelStateBar Instance;
    [SerializeField] private RectTransform bar;
    [SerializeField] private RectTransform markerParent;
    [SerializeField] private UILevelWaveMarker markerPrefab;
    [SerializeField] private Image filler;

    List<UILevelWaveMarker> markers = new();

    private float totalDuration;
    private float time = 0f;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        WaveManager.Instance.OnWaveFirstSpawnEvent += ActivateWave;
    }
    public void Build(LevelData level)
    {
        Clear();

        totalDuration = level.GetTotalDuration();
        float elapsed = 0f;

        List<float> rawTimes = new List<float>();

        for (int i = 0; i < level.waves.Count; i++)
        {
            if (i > 0)
            {
                UILevelWaveMarker marker = Instantiate(markerPrefab, markerParent);
                marker.Setup((elapsed + level.waves[i].wave.spawnEvents[0].time) / totalDuration, i == level.waves.Count - 1);
                markers.Add(marker);
            }

            elapsed = elapsed + level.waves[i].wave.GetDuration();

            if (i < level.waves.Count - 1)
                elapsed += level.waves[i].intervalAfterWave;
        }
        time = 0f;

        /*float duration = rawTimes[^1];

        for (int i = 0; i < rawTimes.Count; i++)
        {
            float normalizedTime = rawTimes[i] / duration;

            UILevelWaveMarker marker = Instantiate(markerPrefab, markerParent);
            marker.Setup(normalizedTime, i == rawTimes.Count - 1);
            markers.Add(marker);
        }*/
    }
    private void Update()
    {
        time += Time.deltaTime;
        filler.fillAmount = Mathf.Clamp01(time / totalDuration);
    }
    void Clear()
    {
        foreach (Transform t in markerParent)
        {
            if (t != filler.transform)
            {
                Destroy(t.gameObject);
            }
        }

        markers.Clear();
    }

    public void ActivateWave(int index)
    {
        markers[index - 1].Activate();
    }
}

