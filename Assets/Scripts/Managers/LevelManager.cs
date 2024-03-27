using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField]
    private List<WorldType> worldOrder = new() { WorldType.Grass, WorldType.Cave, WorldType.Hell };

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

        var sortedLevels = SortLevels(allLevelsData.Levels);
        levels.AddRange(sortedLevels);
    }

    private List<LevelData> SortLevels(List<LevelData> unsortedLevels)
    {
        List<LevelData> output = new();

        Dictionary<WorldType, List<LevelData>> map = new();
        foreach (WorldType wt in worldOrder)
        {
            map.Add(wt, new());
        }

        foreach (var level in unsortedLevels)
        {
            map[level.worldType].Add(level);
        }

        int curIndex = 0;

        foreach (var wt in worldOrder)
        {
            var curLevels = map[wt];
            curLevels.Sort((a, b) => SceneNameComparator(a, b));

            curLevels.ForEach(
                (a) =>
                {
                    a.index = curIndex;
                    curIndex++;
                }
            );

            output.AddRange(curLevels);
        }

        return output;
    }

    private void LoadProgress()
    {
        // TODO: Load data from JSON/Some persistent format
    }

    private void SaveProgress()
    {
        // TODO: Save data to JSON/Some persistent format
    }

    private int SceneNameComparator(LevelData a, LevelData b)
    {
        string sceneName1 = a.sceneName;
        string sceneName2 = b.sceneName;

        if (sceneName1 == null || sceneName2 == null)
        {
            throw new NullReferenceException(
                "Null sceneName in LevelData during LevelManager initialization"
            );
        }

        int suffixIndex1 = sceneName1.LastIndexOf('_');
        int suffixIndex2 = sceneName2.LastIndexOf('_');

        if (suffixIndex1 != -1 && suffixIndex2 != -1)
        {
            string suffixStr1 = sceneName1[(suffixIndex1 + 1)..];
            string suffixStr2 = sceneName2[(suffixIndex2 + 1)..];

            if (
                int.TryParse(suffixStr1, out int suffix1)
                && int.TryParse(suffixStr2, out int suffix2)
            )
            {
                return suffix1.CompareTo(suffix2);
            }
        }

        throw new ArgumentException(
            $"Encountered incorrect level name during LevelManager initialization: {sceneName1}, {sceneName2}"
        );
    }
}
