using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelGoal : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            LevelManager.Instance.CompleteCurrentLevel();
        }
    }
}
