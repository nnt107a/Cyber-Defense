using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    public static TurretManager Instance;
    public List<TurretSaveData> turretDatas = new List<TurretSaveData>();
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
    public void SyncData(List<TurretSaveData> turretSaveDatas)
    {
        turretDatas = turretSaveDatas;
    }
}
