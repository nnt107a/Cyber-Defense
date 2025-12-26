using UnityEngine;

public class StatusEffectHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem slowEffect;
    [SerializeField] private ParticleSystem reduceResEffect;
    public void PlaySlowEffect(bool play)
    {
        if (play)
        {
            if (!slowEffect.isPlaying)
                slowEffect.Play();
        }
        else
        {
            slowEffect.Stop();
        }
    }
    public void PlayReduceResEffect(bool play)
    {
        if (play)
        {
            if (!reduceResEffect.isPlaying)
                reduceResEffect.Play();
        }
        else
        {
            reduceResEffect.Stop();
        }
    }
}
