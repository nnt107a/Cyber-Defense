using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]
public class SaveData
{
    public int eCrystal = 0;
    public List<string> unlockedTechs = new List<string>();
    public int currentLevelIndex = 0;
    public List<TurretSaveData> turretSaveDatas = new List<TurretSaveData>();
}

public static class BinarySaveSystem
{
    private static string path = Path.Combine(Application.persistentDataPath, "game_save.dat");

    public static void Save(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);

        stream.Close();

        Debug.Log("Save file at path: " + path);
    }

    public static SaveData Load()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            try
            {
                SaveData data = formatter.Deserialize(stream) as SaveData;
                stream.Close();
                return data;
            }
            catch
            {
                Debug.LogWarning("Failed to load save data. The file may be corrupted. Creating a new save data.");
                stream.Close();
                return new SaveData();
            }
        }
        else
        {
            Debug.Log("Save file does not exist at path: " + path + ". Creating new save file with default data.");
            SaveData defaultData = CreateDefaultSaveData();
            Save(defaultData);
            return defaultData;
        }
    }

    private static SaveData CreateDefaultSaveData()
    {
        SaveData defaultData = new SaveData();
        // Customize default data here
        defaultData.eCrystal = 100;
        defaultData.unlockedTechs = new List<string>();
        defaultData.currentLevelIndex = 0;
        defaultData.turretSaveDatas = new List<TurretSaveData>();
        int i = 0;
        foreach (var turret in GameManager.Instance.turretDatas)
        {
            TurretSaveData turretSaveData = new TurretSaveData(turret.GetInstanceID(), 1, i++ < 2 ? true : false);
            defaultData.turretSaveDatas.Add(turretSaveData);
        }
        return defaultData;
    }

    public static void DeleteSave()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted at path: " + path);
        }
    }
}
