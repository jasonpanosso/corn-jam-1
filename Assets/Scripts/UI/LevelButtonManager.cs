using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonParent;

    void Start()
    {
        CreateLevelButtons();
    }
    void CreateLevelButtons()
    {
        foreach (Level level in ServiceLocator.LevelManager.Levels)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = "Level " + level.index;

            if (level.completed)
            {
                button.interactable = true;
                button.onClick.AddListener(() => LoadLevel(level.index));
            }
            else
            {
                button.interactable = false;
            }
        }
    }

    void LoadLevel (int levelNumber)
    {
        Debug.Log("Loading Level" + levelNumber);
    }
}
