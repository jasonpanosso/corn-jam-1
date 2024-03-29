using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public void StartLevel0()
    {
        ServiceLocator.LevelManager.LoadLevel(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
