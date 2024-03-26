using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public List<LevelData> Levels { get; private set; }
    public LevelData CurrentLevel { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadProgress();
        LoadLevelData();
        Debug.Log(Levels.Count);
    }

    private void LoadLevelData()
    {
        Levels = Resources.LoadAll<LevelData>("LevelData").ToList();
        Levels.Sort(
            delegate(LevelData a, LevelData b)
            {
                return a.levelIndex.CompareTo(b.levelIndex);
            }
        );
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= Levels.Count)
        {
            Debug.LogError(
                $"levelIndex passed to LevelManager.LoadLevel out of range: {levelIndex}"
            );
            return;
        }

        LevelData levelData = Levels[levelIndex];
        SceneManager.LoadScene(levelData.sceneName);
        CurrentLevel = levelData;
    }

    public void CompleteCurrentLevel()
    {
        CurrentLevel.completed = true;
        if (CurrentLevel.levelIndex + 1 < Levels.Count)
            LoadLevel(CurrentLevel.levelIndex + 1);
        else
            Debug.LogWarning("Unimplemented: Game ending");
    }

    private void LoadProgress()
    {
        // TODO
    }

    private void SaveProgress()
    {
        // TODO
    }
}
