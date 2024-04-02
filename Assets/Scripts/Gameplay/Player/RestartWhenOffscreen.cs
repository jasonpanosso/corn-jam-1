using UnityEngine;

[RequireComponent(typeof(OffScreenDetector))]
public class RestartWhenOffscreen : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<OffScreenDetector>().OnGameObjectOffscreen += Restart;
    }

    private void OnDisable()
    {
        GetComponent<OffScreenDetector>().OnGameObjectOffscreen -= Restart;
    }

    private void Restart(GameObject _)
    {
        ServiceLocator.LevelManager.RestartLevel();
    }
}
