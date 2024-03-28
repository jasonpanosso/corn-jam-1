using System;
using System.Collections.Generic;
using System.IO;
using LDtkUnity;
using LDtkUnity.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LDtkSceneCreator : LDtkPostprocessor
{
    private readonly string levelScenesPath = "Assets/Scenes/Levels/";
    private readonly string allLevelsDataPath = "Assets/Resources/AllLevelsData.asset";
    private readonly string cameraPrefabPath = "Assets/Prefabs/Cameras.prefab";

    private readonly List<LevelData> levelData = new();

    private bool firstRun = true;

    private readonly List<Scene> existingScenesToSave = new();
    private readonly List<(Scene, string)> newScenesToSave = new();

    protected override void OnPostprocessLevel(GameObject root, LdtkJson projectJson)
    {
        if (firstRun)
        {
            firstRun = false;

            SaveAllLevelsDataAfterDelay();
            SaveAndCloseScenesAfterDelay();
        }

        WorldType worldType = GetWorldTypeFromLevelName(root.name);
        string filePath = Path.Combine(levelScenesPath, worldType.ToString(), root.name + ".unity");

        if (File.Exists(filePath))
            EditScene(filePath, root, worldType);
        else
            CreateScene(filePath, root, worldType);
    }

    private void EditScene(string filePath, GameObject level, WorldType worldType)
    {
        Debug.Log($"Existing level {level.name} found, editing scene {filePath}");

        var scene = EditorSceneManager.OpenScene(filePath, OpenSceneMode.Additive);
        foreach (var go in scene.GetRootGameObjects())
        {
            if (go.TryGetComponent<LDtkComponentLevel>(out var _))
            {
                GameObject.DestroyImmediate(go);
                break;
            }
        }

        levelData.Add(new(level.name, worldType));

        level.transform.position = Vector3.zero;
        GameObject.Instantiate(level, scene).name = "LDtkLevel";

        existingScenesToSave.Add(scene);
    }

    private void CreateScene(string filePath, GameObject level, WorldType worldType)
    {
        Debug.Log($"New level {level.name} found, initializing scene {filePath}");

        string parentDirectory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(parentDirectory))
            Directory.CreateDirectory(parentDirectory);

        var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
        levelData.Add(new(level.name, worldType));

        GameObject cameraPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(cameraPrefabPath);
        if (cameraPrefab != null)
        {
            GameObject instance = PrefabUtility.InstantiatePrefab(cameraPrefab) as GameObject;
            SceneManager.MoveGameObjectToScene(instance, newScene);
        }
        else
        {
            Debug.LogError("Failed to load prefab at path: " + cameraPrefabPath);
        }

        level.transform.position = Vector3.zero;
        GameObject.Instantiate(level, newScene).name = "LDtkLevel";

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

    private void SaveAllLevelsDataAfterDelay()
    {
        EditorApplication.delayCall += () =>
        {
            if (levelData.Count == 0)
                return;

            var allLevelsData = InitializeAllLevelsData();
            allLevelsData.Levels = levelData;
            EditorUtility.SetDirty(allLevelsData);
            AssetDatabase.SaveAssetIfDirty(allLevelsData);
        };
    }

    private void SaveAndCloseScenesAfterDelay()
    {
        EditorApplication.delayCall += () =>
        {
            foreach (var scene in existingScenesToSave)
            {
                EditorSceneManager.SaveScene(scene);
                EditorSceneManager.CloseScene(scene, true);
            }

            foreach (var (scene, filePath) in newScenesToSave)
            {
                EditorSceneManager.SaveScene(scene, filePath);
                EditorSceneManager.CloseScene(scene, true);
            }
        };
    }
}
