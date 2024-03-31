using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void PlayCutscene(IEnumerable<CutsceneEvent> cutsceneEvents) =>
        StartCoroutine(PlayCutsceneCoroutine(cutsceneEvents.ToList()));

    private IEnumerator PlayCutsceneCoroutine(List<CutsceneEvent> cutsceneEvents)
    {
        cutsceneEvents.Sort((e1, e2) => e1.startTime.CompareTo(e2.startTime));

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
