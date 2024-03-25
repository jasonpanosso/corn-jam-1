using UnityEngine;

public class SetSpawnPoint : MonoBehaviour
{
    [SerializeField]
    protected GameObject respawnPoint;

    private void Start()
    {
        if (respawnPoint != null)
            return;

        var respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        if (respawnPoints.Length == 0)
        {
            return;
        }
        else if (respawnPoints.Length > 1)
        {
            Debug.LogError("PlayerController instantiated with >1 Respawn point present in scene");
        }
        else
        {
            respawnPoint = respawnPoints[0];
        }
    }

    private void OnEnable()
    {
        if (respawnPoint != null)
            transform.position = respawnPoint.transform.position;
    }
}
