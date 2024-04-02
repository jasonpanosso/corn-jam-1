using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-20)]
public class LevelManager : GenericSingletonMonoBehaviour<LevelManager>
{
    public event Action OnLevelComplete = delegate { };
    public event Action OnLevelLoaded = delegate { };
    public event Action OnLevelLoadBegin = delegate { };

    [SerializeField]
    private string allLevelsDataResourcePath = "AllLevelsData";

    public List<Level> Levels { get; private set; } = new();
    public Level CurrentLevel { get; private set; }

    private void Awake()
    {
        InitializeLevels();
        TrySetCurrentLevelFromSceneName();
    }

    public void LoadLevel(int levelIndex)
    {
        OnLevelLoadBegin.Invoke();

        if (levelIndex < 0 || levelIndex >= Levels.Count)
            Debug.LogError(
                $"levelIndex passed to LevelManager.LoadLevel out of range: {levelIndex}"
            );

        Level nextLevel = Levels[levelIndex];
        StartCoroutine(LoadLevelAsync(nextLevel));
    }

    private IEnumerator LoadLevelAsync(Level nextLevel)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevel.sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        CurrentLevel = nextLevel;
        OnLevelLoaded.Invoke();
    }

    public void LoadNextLevel() => LoadLevel(CurrentLevel.index + 1);

    public void RestartLevel() => LoadLevel(CurrentLevel.index);

    public void CompleteCurrentLevel()
    {
        CurrentLevel.completed = true;
        SaveProgress();
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

        LoadProgress(allLevelsData.Levels);
    }

    private void TrySetCurrentLevelFromSceneName()
    {
        Scene scene = SceneManager.GetActiveScene();
        // Temp hack for menu
        if (scene.name == "Menu")
        {
            CurrentLevel = Levels[0];
            return;
        }

        var cur = Levels.Find(l => l.sceneName == scene.name);

        if (cur == null)
            Debug.Log("Current scene is not a valid level, defaulting to level 0");

        CurrentLevel = cur ?? Levels[0];
    }

    private void LoadProgress(IEnumerable<Level> levelsWithoutProgression)
    {
        var levelsWithProgression = PersistentLevelProgressDataStorage.LoadLevelProgression(
            levelsWithoutProgression
        );

        Levels = levelsWithProgression.ToList();
    }

    private void SaveProgress() => PersistentLevelProgressDataStorage.SaveLevelProgress(Levels);

#if UNITY_EDITOR
    protected override void OnEnable()
    {
        base.OnEnable();
        Awake();
    }
#endif
}
