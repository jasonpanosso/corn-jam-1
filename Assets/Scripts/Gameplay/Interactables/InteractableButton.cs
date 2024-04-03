using System.Collections.Generic;
using UnityEngine;

public class InteractableButton : Interactable
{
    [SerializeField]
    private List<Interactable> interactables = new();

    public override void Interact(GameObject _)
    {
        Debug.Log("interacted with button");

        foreach (var interactable in interactables)
            interactable.Interact(gameObject);
    }
}
