using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private LevelData[] levels;

    private LevelData currentLevel;

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
    }

    private void LoadLevelData()
    {
        levels = Resources.LoadAll<LevelData>("LevelData");
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levels.Length)
        {
            LevelData levelData = levels[levelIndex];
            if (levelData.unlocked)
            {
                SceneManager.LoadScene(levelData.sceneAsset.name);
            }
        }
    }

    public void CompleteLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levels.Length)
        {
            LevelData levelData = levels[levelIndex];
            levelData.completed = true;
            SaveProgress();
        }
    }

    private void LoadProgress() { }

    private void SaveProgress() { }
}
