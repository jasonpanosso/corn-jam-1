[System.Serializable]
public class LevelSaveData
{
    public int index;
    public bool completed = false;
    public bool unlocked = false;
    public int stars = 0;

    public LevelSaveData(
        int index = 0,
        bool completed = false,
        bool unlocked = false,
        int stars = 0
    )
    {
        this.index = index;
        this.completed = completed;
        this.unlocked = unlocked;
        this.stars = stars;
    }
}
