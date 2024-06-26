using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LDtkUnity;
using LDtkUnity.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// Initializes empty scenes for new LDtk levels, and uses level naming convention
// to setup WorldOrderData.
public class LDtkLevelPostprocessor : LDtkPostprocessor
{
    private const string levelScenesPath = "Assets/Scenes/Levels/";
    private const string allLevelsDataPath = "Assets/Resources/AllLevelsData.asset";
    private const string worldOrderDataPath = "Assets/Data/WorldOrderData.asset";

    private readonly List<Level> levels = new();
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
        levels.Add(new(root.name, worldType));

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
        newScene.name = level.name;

        newScenesToSave.Add((newScene, filePath));
        AddSceneToBuildSettingsAfterDelay(filePath);
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

    private void AddSceneToBuildSettingsAfterDelay(string scenePath)
    {
        EditorApplication.delayCall += () =>
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
        };
    }

    private void SaveAllLevelsDataAfterDelay()
    {
        EditorApplication.delayCall += () =>
        {
            var distinctLevels = levels.DistinctBy(level => level.sceneName);

            if (distinctLevels.Count() == 0)
                return;

            var allLevelsData = AssetDatabase.LoadAssetAtPath<AllLevelsData>(allLevelsDataPath);
            var mergedLevels = distinctLevels.Union(
                allLevelsData.Levels,
                new LevelEqualityComparer()
            );

            Debug.Log($"Saving {mergedLevels.Count()} levels to AllLevelsData..");

            var worldOrder = AssetDatabase
                .LoadAssetAtPath<WorldOrderData>(worldOrderDataPath)
                .order;

            var sortedLevels = LevelSorter.SortLevelsByWorldAndLevelId(mergedLevels, worldOrder);
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

class LevelEqualityComparer : IEqualityComparer<Level>
{
    public bool Equals(Level x, Level y)
    {
        if (x == null && y == null)
            return true;
        if (x == null || y == null)
            return false;
        return x.sceneName == y.sceneName;
    }

    public int GetHashCode(Level obj)
    {
        return obj.sceneName?.GetHashCode() ?? 0;
    }
}
