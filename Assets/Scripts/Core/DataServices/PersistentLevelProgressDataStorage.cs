using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class PersistentLevelProgressDataStorage
{
    // Unity JsonUtility can not serialize Lists, so we need this awesome wrapper class : )
    [Serializable]
    private class LevelSaveDataWrapper
    {
        [SerializeField]
        public LevelSaveData[] levelSaveData;
    }

    private const string LEVEL_DATA_PATH = "/levelprogression.json";

    public static bool SaveLevelSaveData(LevelSaveData saveData)
    {
        string path = Application.persistentDataPath + LEVEL_DATA_PATH;

        try
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"{path} does not exist");

            LevelSaveData[] data = JsonUtility
                .FromJson<LevelSaveDataWrapper>(File.ReadAllText(path))
                .levelSaveData;

            int id = Array.FindIndex(data, (el) => el.index == saveData.index);

            if (id == -1)
            {
                Debug.LogError("Attempted to save level with an unknown index");
                return false;
            }

            data[id] = saveData;
            SaveJsonLevelSaveData(data);
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }

        return true;
    }

    public static IEnumerable<LevelSaveData> LoadLevelSaveData(IEnumerable<Level> levels)
    {
        string path = Application.persistentDataPath + LEVEL_DATA_PATH;

        if (!File.Exists(path))
            return SetupDefaultLevelProgression(levels);

        try
        {
            var data = JsonUtility.FromJson<LevelSaveDataWrapper>(File.ReadAllText(path));
            return data.levelSaveData;
        }
        catch
        {
            Debug.LogError($"Failed to load level progression, defaulting to initial data..");
            var initial = new List<LevelSaveData>();
            for (var i = 0; i < 20; i++)
                initial.Add(new(i, false, false, 0));

            initial[0].unlocked = true;
            return initial;
        }
    }

    private static IEnumerable<LevelSaveData> SetupDefaultLevelProgression(
        IEnumerable<Level> levels
    )
    {
        List<LevelSaveData> saveData = new();
        foreach (var level in levels)
            saveData.Add(new(level.index, false, false, 0));

        SaveJsonLevelSaveData(saveData);

        return saveData;
    }

    private static bool SaveJsonLevelSaveData(IEnumerable<LevelSaveData> saveData)
    {
        string path = Application.persistentDataPath + LEVEL_DATA_PATH;

        LevelSaveDataWrapper data = new() { levelSaveData = saveData.ToArray() };

        try
        {
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonUtility.ToJson(data));

            Debug.Log($"Successfully saved data to file {path}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }
}
