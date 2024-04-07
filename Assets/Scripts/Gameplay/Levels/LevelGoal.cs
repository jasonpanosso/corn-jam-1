using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class LevelGoal : MonoBehaviour
{
    public UnityEvent OnEnter;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!hasTriggered && collider.CompareTag("Player"))
        {
            OnEnter.Invoke();
            hasTriggered = true;

            var shotsFired = collider.GetComponent<ShotCounter>().ShotsFired;
            ServiceLocator.LevelManager.CompleteCurrentLevel(shotsFired);
        }
    }
}
