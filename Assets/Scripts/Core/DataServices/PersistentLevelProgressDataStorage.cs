using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class PersistentLevelProgressDataStorage
{
    // Unity JsonUtility can not serialize Lists, so we need this awesome wrapper class : )
    [System.Serializable]
    private class LevelCollection
    {
        [SerializeField]
        public Level[] levels;
    }

    private const string LEVEL_DATA_PATH = "/levelprogress.json";

    private static readonly IDataService dataService = new JsonDataService();

    public static bool SaveLevelProgress(IEnumerable<Level> levels)
    {
        if (levels.Count() == 0)
        {
            Debug.LogWarning(
                "Attempted to save level progress with empty levels collection. Aborting."
            );

            return false;
        }

        LevelCollection col = new() { levels = levels.ToArray() };
        return dataService.SaveData(LEVEL_DATA_PATH, col);
    }

    public static IEnumerable<Level> LoadLevelProgression(IEnumerable<Level> levels)
    {
        string path = Application.persistentDataPath + LEVEL_DATA_PATH;

        if (!File.Exists(path))
        {
            Debug.Log("Progress file not found, initializing with default data set..");
            SaveLevelProgress(levels);
        }

        try
        {
            // TODO/FIXME
            var loaded = dataService.LoadData<LevelCollection>(LEVEL_DATA_PATH).levels;
            if (loaded.Count() == 0)
                return levels;
            else
                return loaded;
        }
        catch
        {
            Debug.LogError($"Failed to load level progression, defaulting to initial data..");
            return levels;
        }
    }
}
