using System.Collections.Generic;
using UnityEngine;

public class InteractableToggler : Interactable
{
    [SerializeField]
    private List<Interactable> interactablesToToggle = new();

    [SerializeField]
    private float timerLength = 10f;

    private float curTimer = 0f;

    private bool isCountingDown = false;

    private void Update()
    {
        if (!isCountingDown)
            return;

        curTimer -= Time.deltaTime;

        if (curTimer <= 0f)
        {
            ToggleAll();
            isCountingDown = false;
        }
    }

    public override void Interact(GameObject _)
    {
        curTimer = timerLength;

        if (!isCountingDown)
        {
            ToggleAll();
            isCountingDown = true;
        }
    }

    private void ToggleAll()
    {
        foreach (var interactable in interactablesToToggle)
            interactable.Interact(gameObject);
    }
}
