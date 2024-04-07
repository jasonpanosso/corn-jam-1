using System;
using UnityEngine;

public static class StarDataService
{
    private const string RESOURCES_DIR = "StarData";

    private static LevelStarData[] _starData;

    public static LevelStarData[] StarData
    {
        get
        {
            if (_starData == null)
            {
                _starData = Resources.LoadAll<LevelStarData>(RESOURCES_DIR);
                Array.Sort(_starData, (a, b) => a.levelIndex > b.levelIndex ? 1 : -1);
            }

            return _starData;
        }
        private set { _starData = value; }
    }
}
