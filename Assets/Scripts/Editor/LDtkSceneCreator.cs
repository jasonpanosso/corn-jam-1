using System;
using System.Collections.Generic;
using System.IO;
using LDtkUnity;
using LDtkUnity.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class LDtkSceneCreator : LDtkPostprocessor
{
    private readonly string levelScenesPath = "Assets/Scenes/Levels/";
    private readonly string allLevelsDataPath = "Assets/Resources/AllLevelsData.asset";

    private readonly List<LevelData> newLevels = new();

    private bool firstRun = true;

    protected override void OnPostprocessLevel(GameObject root, LdtkJson projectJson)
    {
        if (firstRun)
        {
            firstRun = false;
            EditorApplication.delayCall += () =>
            {
                if (newLevels.Count == 0)
                    return;

                var allLevelsData = InitializeAllLevelsData();
                allLevelsData.Levels.AddRange(newLevels);
                EditorUtility.SetDirty(allLevelsData);
                AssetDatabase.SaveAssetIfDirty(allLevelsData);
            };
        }

        WorldType worldType = GetWorldTypeFromLevelName(root.name);
        string filePath = Path.Combine(levelScenesPath, worldType.ToString(), root.name + ".unity");

        if (!File.Exists(filePath))
            CreateScene(filePath, root, worldType);
    }

    private WorldType GetWorldTypeFromLevelName(string levelName)
    {
        foreach (WorldType worldType in Enum.GetValues(typeof(WorldType)))
        {
            var worldName = worldType.ToString();
            if (levelName.StartsWith(worldName))
            {
                Debug.Log($"Found world type {worldType} for {levelName}");
                return worldType;
            }
        }

        Debug.LogError("Invalid level name. Level name must be prefixed with a valid WorldType");
        return WorldType.Grass;
    }

    private void CreateScene(string filePath, GameObject level, WorldType worldType)
    {
        Debug.Log($"New level {level.name} found, initializing scene {filePath}");

        string parentDirectory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(parentDirectory))
            Directory.CreateDirectory(parentDirectory);

        var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
        newLevels.Add(new(level.name, worldType));

        level.transform.position = Vector3.zero;
        GameObject.Instantiate(level, newScene).name = "LDtkLevel";

        EditorSceneManager.SaveScene(newScene, filePath);

        EditorSceneManager.CloseScene(newScene, true);
        AddSceneToBuildSettings(filePath);
    }

    private void AddSceneToBuildSettings(string scenePath)
    {
        var buildScenes = EditorBuildSettings.scenes;
        var newBuildScenes = new EditorBuildSettingsScene[buildScenes.Length + 1];

        for (int i = 0; i < buildScenes.Length; i++)
        {
            newBuildScenes[i] = buildScenes[i];
        }

        newBuildScenes[buildScenes.Length] = new EditorBuildSettingsScene(scenePath, true);
        EditorBuildSettings.scenes = newBuildScenes;

        Debug.Log("Scene added to Build Settings: " + scenePath);
    }

    private AllLevelsData InitializeAllLevelsData()
    {
        var allLevelsData = AssetDatabase.LoadAssetAtPath<AllLevelsData>(allLevelsDataPath);
        if (allLevelsData == null)
        {
            allLevelsData = ScriptableObject.CreateInstance<AllLevelsData>();
            AssetDatabase.CreateAsset(allLevelsData, allLevelsDataPath);
        }

        return allLevelsData;
    }
}
