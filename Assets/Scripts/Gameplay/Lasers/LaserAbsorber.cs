using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LaserAbsorber : MonoBehaviour, ILaserTarget
{
    [SerializeField, InterfaceType(typeof(IInteractable))]
    private Object[] _interactables;
    private IInteractable[] interactables;

    private int curEmitterCount = 0;

    private void OnEnable() => interactables = _interactables.OfType<IInteractable>().ToArray();

    public void OnLaserExit()
    {
        curEmitterCount -= 1;
        if (curEmitterCount == 0)
            SafelyInteract();
    }

    public void OnLaserEnter(Direction _)
    {
        if (curEmitterCount == 0)
            SafelyInteract();

        curEmitterCount += 1;
    }

    private void SafelyInteract()
    {
        foreach (var interactable in interactables)
            if (!interactable.IsUnityNull())
                interactable.Interact(gameObject);
    }
}
