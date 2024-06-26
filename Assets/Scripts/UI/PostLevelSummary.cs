using UnityEngine;

public class PostLevelSummary : MonoBehaviour
{
    [SerializeField]
    private GameObject postLevelPanel;

    private StarAnimator anim;

    public void Show(int stars)
    {
        postLevelPanel.SetActive(true);
        anim.ShowStars(stars);
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

    private void Awake()
    {
        anim = GetComponent<StarAnimator>();
    }

    private void OnEnable() => ServiceLocator.LevelManager.OnLevelComplete += Show;

    private void OnDisable() => ServiceLocator.LevelManager.OnLevelComplete -= Show;
}
