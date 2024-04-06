using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : MonoBehaviour, IInteractable
{
    public UnityEvent OnActivation;

    [SerializeField, InterfaceType(typeof(IInteractable))]
    private Object[] _interactables;
    private IInteractable[] interactables;

    private void OnEnable() => interactables = _interactables.OfType<IInteractable>().ToArray();

    public void Interact(GameObject _)
    {
        OnActivation.Invoke();
        foreach (var interactable in interactables)
            interactable.Interact(gameObject);
    }
}
