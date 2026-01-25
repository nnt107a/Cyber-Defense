using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int eCrystal = 0;
    public List<string> unlockedTechs = new List<string>();
    public int currentLevelIndex = 0;

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
                Debug.LogError("Failed to load save data. The file may be corrupted. Creating a new save data.");
                stream.Close();
                return new SaveData();
            }
        }
        else
        {
            return new SaveData();
        }
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
