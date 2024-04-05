using System.Linq;
using UnityEngine;

public class LaserAbsorber : MonoBehaviour, ILaserTarget
{
    [SerializeField, InterfaceType(typeof(IInteractable))]
    private Object[] _interactables;
    private IInteractable[] interactables;
    private void Awake() => interactables = _interactables.OfType<IInteractable>().ToArray();

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
