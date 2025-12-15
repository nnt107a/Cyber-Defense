using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VenomSpireZone : MonoBehaviour, IPoolable
{
    [SerializeField] private EffectData venomEffectData;
    private GameObject prefabRef;
    private float timer = 0f;
    private bool alive = false;
    private HashSet<Collider2D> enemiesInRange = new();
    private new Collider2D collider2D;
    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }
    public void Init(GameObject prefab)
    {
        prefabRef = prefab;
        timer = -0.1f;
    }
    private void Update()
    {
        if (!alive)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer > venomEffectData.duration)
        {
            ObjectPool.Instance.Despawn(prefabRef, gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision);
        }
    }
    public void OnDespawn()
    {
        foreach (var enemyCollider in enemiesInRange.ToList())
        {
            OnTriggerExit2D(enemyCollider);
        }
        enemiesInRange.Clear();
        alive = false;
        collider2D.enabled = false;
        StopAllCoroutines();
    }

    public void OnSpawn()
    {
        alive = true;
        collider2D.enabled = true;
        StartCoroutine(ApplyDamageOverTime());
    }
    private IEnumerator ApplyDamageOverTime()
    {
        while (alive)
        {
            foreach (var enemyCollider in enemiesInRange.ToList())
            {
                Debug.Log("Venom Spire Zone applying damage to " + enemyCollider.name + " for " + venomEffectData.effectValue + " damage");
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
