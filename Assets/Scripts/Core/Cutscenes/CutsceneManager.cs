using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutsceneManager : GenericSingletonMonoBehaviour<CutsceneManager>
{
    public void PlayCutscene(IEnumerable<CutsceneEvent> cutsceneEvents) =>
        StartCoroutine(PlayCutsceneCoroutine(cutsceneEvents));

    private IEnumerator PlayCutsceneCoroutine(IEnumerable<CutsceneEvent> cutsceneEvents)
    {
        cutsceneEvents.ToList().Sort((e1, e2) => e1.startTime.CompareTo(e2.startTime));

        foreach (var e in cutsceneEvents)
        {
            yield return new WaitForSeconds(e.startTime);
            e.Init();

            while (!e.IsFinished())
            {
                e.Update(Time.deltaTime);
                yield return null;
            }
        }

        // cleanup?
    }
}
