using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public void StartLevel0()
    {
        LevelManager.Instance.LoadLevel(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
