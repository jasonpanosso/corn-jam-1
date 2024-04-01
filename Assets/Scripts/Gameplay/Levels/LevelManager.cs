using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : GenericSingletonMonoBehaviour<LevelManager>
{
    public event Action OnLevelComplete = delegate { };
    public event Action OnLevelLoad = delegate { };

    [SerializeField]
    private string allLevelsDataResourcePath = "AllLevelsData";

    public readonly List<Level> levels = new();
    public Level CurrentLevel { get; private set; }

    private void Awake()
    {
        InitializeLevels();
        TrySetCurrentLevelFromSceneName();
        LoadProgress();
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
            Debug.LogError(
                $"levelIndex passed to LevelManager.LoadLevel out of range: {levelIndex}"
            );

        Level nextLevel = levels[levelIndex];
        SceneManager.LoadScene(nextLevel.sceneName);
        CurrentLevel = nextLevel;
        OnLevelLoad.Invoke();
    }

    public void LoadNextLevel() => LoadLevel(CurrentLevel.index + 1);

    public void RestartLevel() => LoadLevel(CurrentLevel.index);

    public void CompleteCurrentLevel()
    {
        CurrentLevel.completed = true;
        OnLevelComplete.Invoke();
    }

    private void InitializeLevels()
    {
        var allLevelsData = Resources.Load<AllLevelsData>(allLevelsDataResourcePath);
        if (allLevelsData == null)
        {
            Debug.LogError(
                "AllLevelsData scriptable object not found during LevelManager initialization"
            );
            return;
        }

        levels.AddRange(allLevelsData.Levels);
    }

    private void TrySetCurrentLevelFromSceneName()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Menu")
        {
            CurrentLevel = levels[0];
            return;
        }

        var cur = levels.Find(l => l.sceneName == scene.name);

        if (cur == null)
            Debug.Log("Current scene is not a valid level, defaulting to level 0");

        CurrentLevel = cur ?? levels[0];
    }

    private void LoadProgress()
    {
        // TODO: Load data from JSON/Some persistent format
    }

    private void SaveProgress()
    {
        // TODO: Save data to JSON/Some persistent format
    }

#if UNITY_EDITOR
    protected override void OnEnable()
    {
        base.OnEnable();
        EditorCleanup();
        Awake();
    }

    private void EditorCleanup() => levels.Clear();
#endif
}
