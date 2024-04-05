[System.Serializable]
public class Level
{
    public int index;
    public WorldType worldType;
    public string name = "";
    public string sceneName;
    public bool completed = false;
    public bool unlocked = false;

    public Level(
        string sceneName,
        WorldType worldType,
        int index = 0,
        string name = "",
        bool completed = false,
        bool unlocked = false
    )
    {
        this.sceneName = sceneName;
        this.worldType = worldType;
        this.index = index;
        this.name = name;
        this.completed = completed;
        this.unlocked = unlocked;
    }
}
