using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;

    void Start()
    {
        if (playerPrefab == null)
            Debug.LogError("Player Prefab not assigned in PlayerSpawner");
        else
            Instantiate(playerPrefab, transform.position, playerPrefab.transform.rotation);
    }
}
