using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary
        = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();
        }

        GameObject obj;

        if (poolDictionary[prefab].Count > 0)
        {
            obj = poolDictionary[prefab].Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
        }
        else
        {
            obj = Instantiate(prefab, position, rotation);
        }

        obj.SetActive(true);

        if (obj.TryGetComponent(out IPoolable poolable))
            poolable.OnSpawn();

        return obj;
    }

    public void Despawn(GameObject prefab, GameObject obj)
    {
        if (obj.TryGetComponent(out IPoolable poolable))
            poolable.OnDespawn();

        obj.SetActive(false);

        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();
        }

        poolDictionary[prefab].Enqueue(obj);
    }
}
