using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuPanel;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        var pi = FindFirstObjectByType<PlayerInput>();
        if (pi != null)
            pi.EnableInput();
    }

    public void Pause()
    {
        PauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        var pi = FindFirstObjectByType<PlayerInput>();
        if (pi != null)
            pi.DisableInput();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        isPaused = false;
        ServiceLocator.LevelManager.RestartLevel();
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
