using System.Collections.Generic;
using UnityEngine;

public class SoundLimiter : MonoBehaviour
{
    public static SoundLimiter Instance;

    private class SoundData
    {
        public int playCount;
        public float windowStartTime;
    }

    private Dictionary<AudioClip, SoundData> soundMap = new();

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

    public bool CanPlay(AudioClip clip, int maxPlays, float timeWindow)
    {
        float now = Time.time;

        if (!soundMap.TryGetValue(clip, out SoundData data))
        {
            data = new SoundData
            {
                playCount = 0,
                windowStartTime = now
            };
            soundMap[clip] = data;
        }

        if (now - data.windowStartTime > timeWindow)
        {
            data.playCount = 0;
            data.windowStartTime = now;
        }

        if (data.playCount >= maxPlays)
            return false;

        data.playCount++;
        return true;
    }
}
