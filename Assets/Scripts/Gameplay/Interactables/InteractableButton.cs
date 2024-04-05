using System.Linq;
using UnityEngine;

public class InteractableButton : MonoBehaviour, IInteractable
{
    [SerializeField, InterfaceType(typeof(IInteractable))]
    private Object[] _interactables;
    private IInteractable[] interactables;

    private void Awake() =>
        interactables = _interactables.OfType<IInteractable>().ToArray();

    public void Interact(GameObject _)
    {
        foreach (var interactable in interactables)
            interactable.Interact(gameObject);
    }
}
