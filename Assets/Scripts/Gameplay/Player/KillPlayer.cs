using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class KillPlayer : MonoBehaviour
{
    public UnityEvent OnKill;

    private bool isKilled = false;

    public void Kill()
    {
        if (isKilled)
            return;

        isKilled = true;
        OnKill.Invoke();
        StartCoroutine(WaitUntilRestart(1.5f));
    }

    private IEnumerator WaitUntilRestart(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ServiceLocator.LevelManager.RestartLevel();
    }
}
