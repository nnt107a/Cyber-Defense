using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Action<int> OnECoreCountChanged;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private int eCoreCountInit = 100;
    [HideInInspector]
    public int eCoreCount = 0;
    public void Init()
    {
        eCoreCount = eCoreCountInit;
        ChangeECoreCount(0);
    }
    public void ChangeECoreCount(int amount)
    {
        eCoreCount += amount;
        OnECoreCountChanged?.Invoke(eCoreCount);
    }
}
