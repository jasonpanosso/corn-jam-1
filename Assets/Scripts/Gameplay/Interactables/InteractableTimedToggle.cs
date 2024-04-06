using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InteractableToggler : MonoBehaviour, IInteractable
{
    public UnityEvent OnActivation;
    public UnityEvent OnDeactivation;

    [SerializeField, InterfaceType(typeof(IInteractable))]
    private Object[] _interactablesToToggle;
    private IInteractable[] interactablesToToggle;

    [SerializeField]
    private float timerLength = 10f;

    private float curTimer = 0f;

    private bool isCountingDown = false;

    private void OnEnable() =>
        interactablesToToggle = _interactablesToToggle.OfType<IInteractable>().ToArray();

    private void Update()
    {
        if (!isCountingDown)
            return;

        curTimer -= Time.deltaTime;

        if (curTimer <= 0f)
        {
            OnDeactivation.Invoke();
            ToggleAll();
            isCountingDown = false;
        }
    }

    public void Interact(GameObject _)
    {
        curTimer = timerLength;

        if (!isCountingDown)
        {
            OnActivation.Invoke();
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
