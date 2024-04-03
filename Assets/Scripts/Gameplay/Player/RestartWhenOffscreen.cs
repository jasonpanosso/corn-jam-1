using UnityEngine;

[RequireComponent(typeof(OffScreenDetector))]
public class RestartWhenOffscreen : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<OffScreenDetector>().OnGameObjectOffscreen += Restart;
        ServiceLocator.LevelManager.OnLevelComplete += Disable;
    }

    private void OnDisable()
    {
        GetComponent<OffScreenDetector>().OnGameObjectOffscreen -= Restart;
        ServiceLocator.LevelManager.OnLevelComplete -= Disable;
    }

    private void Disable() => enabled = false;

    private void Restart(GameObject _) => ServiceLocator.LevelManager.RestartLevel();
}
