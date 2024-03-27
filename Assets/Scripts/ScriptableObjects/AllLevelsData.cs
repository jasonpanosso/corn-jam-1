using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllLevelsData", menuName = "Custom/All Levels Data")]
public class AllLevelsData : ScriptableObject
{
    public List<LevelData> Levels = new();
}
