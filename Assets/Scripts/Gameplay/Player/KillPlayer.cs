using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class KillPlayer : MonoBehaviour
{
    public UnityEvent OnKill;

    public void Kill()
    {
        OnKill.Invoke();
        StartCoroutine(WaitUntilRestart(3));
    }

    private IEnumerator WaitUntilRestart(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        ServiceLocator.LevelManager.RestartLevel();
    }
}
