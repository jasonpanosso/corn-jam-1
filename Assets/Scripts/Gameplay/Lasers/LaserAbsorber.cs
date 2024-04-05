using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LaserAbsorber : MonoBehaviour, ILaserTarget
{
    [SerializeField, InterfaceType(typeof(IInteractable))]
    private Object[] _interactables;
    private IInteractable[] interactables;

    private void Awake() => interactables = _interactables.OfType<IInteractable>().ToArray();

    public void OnLaserExit() => SafelyInteract();

    public void OnLaserEnter(Direction _) => SafelyInteract();

    private void SafelyInteract()
    {
        foreach (var interactable in interactables)
            if (!interactable.IsUnityNull())
                interactable.Interact(gameObject);
    }
}
