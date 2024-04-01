using System;
using System.IO;
using UnityEngine;

public class JsonDataService : IDataService
{
    public bool SaveData<T>(string relativePath, T data)
    {
        string path = Application.persistentDataPath + relativePath;

        try
        {
            if (File.Exists(path))
                File.Delete(path);

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

    public T LoadData<T>(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;

        if (!File.Exists(path))
        {
            Debug.LogError($"Cannot load file at {path}. File does not exist");
            throw new FileNotFoundException($"{path} does not exist");
        }

        try
        {
            var data = JsonUtility.FromJson<T>(File.ReadAllText(path));

            Debug.Log($"Successfully loaded data from file {path}");
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }
}
