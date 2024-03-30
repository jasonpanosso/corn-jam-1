using UnityEngine;

public class PostLevelSummary : MonoBehaviour
{
    [SerializeField]
    private GameObject postLevelPanel;

    private StarAnimator anim;

    private void Awake()
    {
        anim = GetComponent<StarAnimator>();
    }

    public void Show(int score)
    {
        postLevelPanel.SetActive(true);
        anim.ShowStars(score);
    }

    public void RetryLevel()
    {
        postLevelPanel.SetActive(false);
        ServiceLocator.LevelManager.RestartLevel();
    }

    public void NextLevel()
    {
        postLevelPanel.SetActive(false);
        ServiceLocator.LevelManager.LoadNextLevel();
    }
}
