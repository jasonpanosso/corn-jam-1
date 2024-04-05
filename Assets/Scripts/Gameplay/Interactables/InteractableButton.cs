using System.Collections.Generic;
using UnityEngine;

public class InteractableButton : MonoBehaviour, IInteractable
{
    [SerializeField]
    private List<IInteractable> interactables = new();

    public void Interact(GameObject _)
    {
        foreach (var interactable in interactables)
            interactable.Interact(gameObject);
    }
}
