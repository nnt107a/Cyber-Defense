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
        WaveManager.Instance.OnWaveLastSpawnEvent += ActivateWave;
    }
    public void Build(LevelData level)
    {
        Clear();

        totalDuration = level.GetTotalDuration();
        float elapsed = 0f;

        for (int i = 0; i < level.waves.Count; i++)
        {
            elapsed += level.waves[i].wave.GetLastSpawnTime();

            float normalizedTime = elapsed / totalDuration;

            UILevelWaveMarker marker = Instantiate(markerPrefab, markerParent);
            marker.Setup(normalizedTime, i == level.waves.Count - 1);
            markers.Add(marker);

            elapsed = elapsed - level.waves[i].wave.GetLastSpawnTime() + level.waves[i].wave.GetDuration();

            if (i < level.waves.Count - 1)
                elapsed += level.waves[i].intervalAfterWave;
        }
        time = 0f;
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
        markers[index].Activate();
    }
}

