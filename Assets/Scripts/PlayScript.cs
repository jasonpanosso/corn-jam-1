using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayScript : MonoBehaviour
{
    public void StartLevel0()
    {
            LevelManager.Instance.LoadLevel(0);      
    }
}
