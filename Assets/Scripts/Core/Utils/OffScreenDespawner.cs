using UnityEngine;

[RequireComponent(typeof(OffScreenDetector))]
public class OffScreenDespawner : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<OffScreenDetector>().OnGameObjectOffscreen += Despawn;
    }

    private void OnDisable()
    {
        GetComponent<OffScreenDetector>().OnGameObjectOffscreen -= Despawn;
    }

    private void Despawn(GameObject _) => Destroy(gameObject);
}
