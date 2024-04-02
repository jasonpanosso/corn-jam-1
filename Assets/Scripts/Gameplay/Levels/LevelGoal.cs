using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelGoal : MonoBehaviour
{
    [SerializeField]
    private string onEnterSFX = "SFX_Sizzle";

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!hasTriggered && collider.CompareTag("Player"))
        {
            hasTriggered = true;
            ServiceLocator.LevelManager.CompleteCurrentLevel();
            ServiceLocator.AudioManager.PlayAudioItem(onEnterSFX);
        }
    }
}
