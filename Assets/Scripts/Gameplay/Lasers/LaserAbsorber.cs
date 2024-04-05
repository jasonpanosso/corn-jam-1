using System.Collections.Generic;
using UnityEngine;

public class LaserAbsorber : MonoBehaviour, ILaserTarget
{
    [SerializeField]
    private List<IInteractable> interactables = new();

    public void OnLaserExit()
    {
        foreach (var interactable in interactables)
            if (interactable != null)
                interactable.Interact(gameObject);
    }

    public void OnLaserEnter(Direction _)
    {
        foreach (var interactable in interactables)
            if (interactable != null)
                interactable.Interact(gameObject);
    }
}
