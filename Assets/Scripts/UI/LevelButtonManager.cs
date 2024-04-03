using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform panelParent;
    public GameObject panelPrefab;
    private List<GameObject> Panels = new();
    public int ButtonsPerRow = 4;
    void Start()
    {
        CreateLevelButtons();
    }
    void CreateLevelButtons()
    {
        int numberOfLevels = ServiceLocator.LevelManager.Levels.Count;
        int numberOfPanels = Mathf.CeilToInt((float) numberOfLevels / ButtonsPerRow);

        for (int i = 0; i < numberOfPanels; i++)
        {
            GameObject panel = Instantiate(panelPrefab, panelParent);
            Panels.Add(panel);
        }

        int currentPanelIndex = 0;
        int buttonCount = 0;
        foreach (Level level in ServiceLocator.LevelManager.Levels)
        {
            if (buttonCount >= ButtonsPerRow)
            {
                currentPanelIndex++;
                buttonCount = 0;
            }
            GameObject buttonObj = Instantiate(buttonPrefab, Panels[currentPanelIndex].transform);
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

            buttonCount++;
        }
    }

    void LoadLevel (int levelNumber)
    {
        Debug.Log("Loading Level" + levelNumber);
    }
}
