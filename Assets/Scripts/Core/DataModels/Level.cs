[System.Serializable]
public class Level
{
    public int index;
    public WorldType worldType;
    public string name = "";
    public string sceneName;

    public Level(string sceneName, WorldType worldType, int index = 0, string name = "")
    {
        this.sceneName = sceneName;
        this.worldType = worldType;
        this.index = index;
        this.name = name;
    }
}
