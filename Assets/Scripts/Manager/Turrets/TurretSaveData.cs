using System;
using UnityEngine;

[Serializable]
public class TurretSaveData
{
    public int turretID;
    public int level;
    public bool unlocked;
    public TurretSaveData(int turretID, int level, bool unlocked)
    {
        this.turretID = turretID;
        this.level = level;
        this.unlocked = unlocked;
    }
    public void UpgradeLevel()
    {
        level++;
    }
}
