using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LDtkUnity;
using LDtkUnity.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// Initializes empty scenes for new LDtk levels, and uses level naming convention
// to setup WorldOrderData.
public class LDtkLevelPostprocessor : LDtkPostprocessor
{
    private readonly string levelScenesPath = "Assets/Scenes/Levels/";
    private readonly string allLevelsDataPath = "Assets/Resources/AllLevelsData.asset";
    private readonly string worldOrderDataPath = "Assets/Data/WorldOrderData.asset";

    private readonly List<LevelData> levelData = new();
    private readonly List<(Scene, string)> newScenesToSave = new();

    private bool firstRun = true;
    private Scene currentActiveScene;

    protected override void OnPostprocessLevel(GameObject root, LdtkJson projectJson)
    {
        if (firstRun)
        {
            firstRun = false;

            currentActiveScene = EditorSceneManager.GetActiveScene();

            SaveAllLevelsDataAfterDelay();
            SaveAndCloseScenesAfterDelay();
        }

        WorldType worldType = GetWorldTypeFromLevelName(root.name);
        string filePath = Path.Combine(levelScenesPath, worldType.ToString(), root.name + ".unity");
        levelData.Add(new(root.name, worldType));

        if (!File.Exists(filePath))
            CreateScene(filePath, root);
    }

    private void CreateScene(string filePath, GameObject level)
    {
        Debug.Log($"New level {level.name} found, initializing scene {filePath}");

        string parentDirectory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(parentDirectory))
            Directory.CreateDirectory(parentDirectory);

        var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);

        newScenesToSave.Add((newScene, filePath));
        AddSceneToBuildSettings(filePath);
    }

    private WorldType GetWorldTypeFromLevelName(string levelName)
    {
        foreach (WorldType worldType in Enum.GetValues(typeof(WorldType)))
        {
            var worldName = worldType.ToString();
            if (levelName.StartsWith(worldName))
            {
                return worldType;
            }
        }

        Debug.LogError("Invalid level name. Level name must be prefixed with a valid WorldType");
        return WorldType.Grass;
    }

    private void AddSceneToBuildSettings(string scenePath)
    {
        var buildScenes = EditorBuildSettings.scenes;
        if (Array.Exists(buildScenes, el => el.path == scenePath))
            return;

        var newBuildScenes = new EditorBuildSettingsScene[buildScenes.Length + 1];

        for (int i = 0; i < buildScenes.Length; i++)
        {
            newBuildScenes[i] = buildScenes[i];
        }

        newBuildScenes[buildScenes.Length] = new EditorBuildSettingsScene(scenePath, true);
        EditorBuildSettings.scenes = newBuildScenes;

        Debug.Log("Scene added to Build Settings: " + scenePath);
    }

    private void SaveAllLevelsDataAfterDelay()
    {
        EditorApplication.delayCall += () =>
        {
            if (levelData.Count == 0)
                return;

            var allLevelsData = AssetDatabase.LoadAssetAtPath<AllLevelsData>(allLevelsDataPath);

            var worldOrder = AssetDatabase
                .LoadAssetAtPath<WorldOrderData>(worldOrderDataPath)
                .order;

            var sortedLevels = LevelSorter.SortLevelsByWorldAndLevelId(levelData, worldOrder);
            allLevelsData.Levels = sortedLevels.ToList();
            EditorUtility.SetDirty(allLevelsData);
            AssetDatabase.SaveAssetIfDirty(allLevelsData);
        };
    }

    private void SaveAndCloseScenesAfterDelay()
    {
        EditorApplication.delayCall += () =>
        {
            foreach (var (scene, filePath) in newScenesToSave)
            {
                EditorSceneManager.SaveScene(scene, filePath);
                if (scene != currentActiveScene)
                {
                    EditorSceneManager.CloseScene(scene, true);
                }
            }
        };
    }
}
