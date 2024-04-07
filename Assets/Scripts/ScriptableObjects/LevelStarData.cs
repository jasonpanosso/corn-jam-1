using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelStarData", menuName = "Custom/Level Star Data")]
public class LevelStarData : ScriptableObject
{
    public uint levelIndex;
    public uint threeStarShots;
    public uint twoStarShots;
    public uint oneStarShots;

    public int GetStarsFromShotsFired(int shotsFired)
    {
        if (shotsFired <= threeStarShots)
            return 3;
        else if (shotsFired <= twoStarShots)
            return 2;
        else if (shotsFired <= oneStarShots)
            return 1;
        else
            return 0;
    }
}
