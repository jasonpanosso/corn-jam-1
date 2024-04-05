using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonPrefab;

    [SerializeField]
    private Transform panelParent;

    [SerializeField]
    private GameObject panelPrefab;

    [SerializeField]
    private int buttonsPerRow = 4;

    private readonly List<GameObject> Panels = new();

    private void Start() => CreateLevelButtons();

    private void CreateLevelButtons()
    {
        int numberOfLevels = ServiceLocator.LevelManager.Levels.Count;
        int numberOfPanels = Mathf.CeilToInt((float)numberOfLevels / buttonsPerRow);

        for (int i = 0; i < numberOfPanels; i++)
        {
            GameObject panel = Instantiate(panelPrefab, panelParent);
            Panels.Add(panel);
        }

        int currentPanelIndex = 0;
        int buttonCount = 0;
        foreach (Level level in ServiceLocator.LevelManager.Levels)
        {
            if (buttonCount >= buttonsPerRow)
            {
                currentPanelIndex++;
                buttonCount = 0;
            }
            GameObject buttonObj = Instantiate(buttonPrefab, Panels[currentPanelIndex].transform);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = "Level " + level.index;

            if (level.unlocked)
                button.interactable = true;
            else
                button.interactable = false;

            button.onClick.AddListener(() => LoadLevel(level.index));
            buttonCount++;
        }
    }

    private void LoadLevel(int levelNumber) => ServiceLocator.LevelManager.LoadLevel(levelNumber);
}
