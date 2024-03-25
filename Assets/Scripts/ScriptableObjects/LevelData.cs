using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Create New Level Data")]
public class LevelData : ScriptableObject
{
    public int levelIndex;
    public string levelName;
    public bool unlocked;
    public bool completed;
    public string sceneName;
}
