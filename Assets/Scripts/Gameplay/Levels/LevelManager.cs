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
        TrySetCurrentLevelFromSceneName();
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

    public void LoadNextLevel()
    {
        var index = CurrentLevel.index;
        if (index >= levels.Count)
        {
            Debug.LogWarning("Unimplemented: Game ending");
            return;
        }

        LevelData levelData = levels[index + 1];
        SceneManager.LoadScene(levelData.sceneName);
        CurrentLevel = levelData;
        FindObjectOfType<PlayerInput>().EnableInput();
    }

    public void CompleteCurrentLevel()
    {
        CurrentLevel.completed = true;

        var playerInput = FindObjectOfType<PlayerInput>();
        var postLevelSummary = FindObjectOfType<PostLevelSummary>();

        playerInput.DisableInput();

        // TODO/FIXME: No scoring system is implemented, simply show three stars
        postLevelSummary.Show(3);
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

    public void RestartLevel()
    {
        LoadLevel(CurrentLevel.index);
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
}
