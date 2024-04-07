using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-20)]
public class LevelManager : GenericSingletonMonoBehaviour<LevelManager>
{
    public event Action<int> OnLevelComplete = delegate { };
    public event Action OnLevelLoaded = delegate { };
    public event Action OnLevelLoadBegin = delegate { };

    [SerializeField]
    private string allLevelsDataResourcePath = "AllLevelsData";

    public List<LevelSaveData> LevelSaveData { get; private set; } = new();
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

        if (levelIndex < 0)
            Debug.LogError(
                $"levelIndex passed to LevelManager.LoadLevel out of range: {levelIndex}"
            );
        else if (levelIndex >= Levels.Count)
        {
            SceneManager.LoadScene("Menu");
            return;
        }

        Level nextLevel = Levels[levelIndex];
        StartCoroutine(LoadLevelAsync(nextLevel));
        CurrentLevel = nextLevel;
    }

    private IEnumerator LoadLevelAsync(Level nextLevel)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevel.sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        OnLevelLoaded.Invoke();
    }

    public void LoadNextLevel() => LoadLevel(CurrentLevel.index + 1);

    public void RestartLevel() => LoadLevel(CurrentLevel.index);

    public void CompleteCurrentLevel(int shotsFired)
    {
        var save = LevelSaveData[CurrentLevel.index];
        save.completed = true;
        save.unlocked = true;

        var starData = StarDataService.StarData[save.index];
        save.stars = starData.GetStarsFromShotsFired(shotsFired);

        SaveProgress(save);

        if (CurrentLevel.index + 1 < Levels.Count)
        {
            var next = LevelSaveData[CurrentLevel.index + 1];
            if (!next.unlocked)
            {
                next.unlocked = true;
                SaveProgress(next);
            }
        }

        OnLevelComplete.Invoke(save.stars);
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

        Levels = allLevelsData.Levels;
        LoadProgress(allLevelsData.Levels);
    }

    private void TrySetCurrentLevelFromSceneName()
    {
        Scene scene = SceneManager.GetActiveScene();
        // Temp hack for menu
        if (scene.name == "Menu")
            return;

        var cur = Levels.Find(l => l.sceneName == scene.name);

        if (cur == null)
            Debug.Log("Current scene is not a valid level, defaulting to level 0");

        CurrentLevel = cur ?? Levels[0];
    }

    private void LoadProgress(IEnumerable<Level> levels)
    {
        var saveData = PersistentLevelProgressDataStorage.LoadLevelSaveData(levels);

        LevelSaveData = saveData.ToList();
        LevelSaveData[0].unlocked = true;
    }

    private void SaveProgress(LevelSaveData toSave) =>
        PersistentLevelProgressDataStorage.SaveLevelSaveData(toSave);

#if UNITY_EDITOR
    protected override void OnEnable()
    {
        base.OnEnable();
        Awake();
    }
#endif
}
