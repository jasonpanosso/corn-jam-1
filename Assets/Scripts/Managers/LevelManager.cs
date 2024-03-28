using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField]
    private string allLevelsDataResourcePath = "AllLevelsData";

    public readonly List<LevelData> levels = new();
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
        InitializeLevels();
        LoadProgress();
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
            Debug.LogError(
                $"levelIndex passed to LevelManager.LoadLevel out of range: {levelIndex}"
            );

        LevelData levelData = levels[levelIndex];
        SceneManager.LoadScene(levelData.sceneName);
        CurrentLevel = levelData;
    }

    public void CompleteCurrentLevel()
    {
        CurrentLevel.completed = true;
        if (CurrentLevel.index + 1 < levels.Count)
            LoadLevel(CurrentLevel.index + 1);
        else
            Debug.LogWarning("Unimplemented: Game ending");
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

    private void LoadProgress()
    {
        // TODO: Load data from JSON/Some persistent format
    }

    private void SaveProgress()
    {
        // TODO: Save data to JSON/Some persistent format
    }
}
