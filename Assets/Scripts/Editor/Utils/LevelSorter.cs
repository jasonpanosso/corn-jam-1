using System;
using System.Collections.Generic;

public static class LevelSorter
{
    public static IEnumerable<LevelData> SortLevelsByWorldAndLevelId(
        IEnumerable<LevelData> unsortedLevels,
        IEnumerable<WorldType> worldOrder
    )
    {
        Dictionary<WorldType, List<LevelData>> map = new();

        foreach (var wt in worldOrder)
            map.Add(wt, new());

        foreach (var level in unsortedLevels)
            map[level.worldType].Add(level);

        List<LevelData> output = new();
        int curIndex = 0;
        foreach (var wt in worldOrder)
        {
            var curLevels = map[wt];
            curLevels.Sort((a, b) => CompareSceneNamesByIntSuffix(a, b));

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

    private static int CompareSceneNamesByIntSuffix(LevelData a, LevelData b)
    {
        string sceneName1 = a.sceneName;
        string sceneName2 = b.sceneName;

        if (sceneName1 == null || sceneName2 == null)
            throw new NullReferenceException("Null sceneName in LevelData during level sorting");

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
                return suffix1.CompareTo(suffix2);
        }

        throw new ArgumentException(
            $"Encountered incorrect level names while sorting levels: {sceneName1}, {sceneName2}"
        );
    }
}
