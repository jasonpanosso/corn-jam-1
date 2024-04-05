using System.Linq;
using UnityEngine;

public class InteractableToggler : MonoBehaviour, IInteractable
{
    [SerializeField, InterfaceType(typeof(IInteractable))]
    private Object[] _interactablesToToggle;
    private IInteractable[] interactablesToToggle;

    [SerializeField]
    private float timerLength = 10f;

    private float curTimer = 0f;

    private bool isCountingDown = false;

    private void Awake() =>
        interactablesToToggle = _interactablesToToggle.OfType<IInteractable>().ToArray();

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

    public void Interact(GameObject _)
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
