using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TurretUnlockData
{
    public int levelIndex;
    public int turretID;
}
public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;
    [SerializeField] private int crystalRewardWin = 100;
    [SerializeField] private int crystalRewardLose = 20;

    [SerializeField] private List<TurretUnlockData> turretUnlockDatas = new();
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
    public int GetCrystalReward(bool isWin)
    {
        return isWin ? crystalRewardWin : crystalRewardLose;
    }
    public void UnlockTurret(int levelIndex)
    {
        foreach (var data in turretUnlockDatas)
        {
            if (data.levelIndex == levelIndex)
            {
                foreach (var turret in GameManager.Instance.currentData.turretSaveDatas)
                {
                    if (turret.turretID == data.turretID)
                    {
                        turret.unlocked = true;
                        break;
                    }
                }
            }
        }
        GameManager.Instance.SaveToDisk();
    }
}
