using System.Collections;
using System.Collections.Generic;
using CareBoo.Serially;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class KernelPopAction : MonoBehaviour, IAction
{
    public UnityEvent OnPop;

    [TypeFilter(derivedFrom: typeof(IInteractable))]
    [SerializeField]
    private List<SerializableType> interactableTypes = new();

    [SerializeField]
    private float popRadius = 2f;

    [SerializeField]
    private float ignoreCollisionDuration = 0.1f;

    private Animator anim;

    public bool HasExecuted
    {
        get => _isPopped;
        set => _isPopped = value;
    }

    private bool _isPopped = false;

    public void Execute()
    {
        if (!HasExecuted)
            Pop();
    }

    private void Pop()
    {
        StartCoroutine(SwapToPopcornLayerAfterDelay());
        if (anim != null)
            anim.SetTrigger("Pop");

        var inRange = Physics2D.OverlapCircleAll(transform.position, popRadius);
        foreach (var hit in inRange)
        {
            foreach (var type in interactableTypes)
            {
                if (hit.TryGetComponent(type.Type, out var component))
                {
                    IInteractable target = component as IInteractable;
                    target.Interact(gameObject);
                }
            }
        }

        OnPop.Invoke();
        HasExecuted = true;
    }

    private IEnumerator SwapToPopcornLayerAfterDelay()
    {
        yield return new WaitForSeconds(ignoreCollisionDuration);

        if (gameObject != null)
            gameObject.layer = LayerMask.NameToLayer("Popcorn");
    }

    private void Awake() => anim = GetComponent<Animator>();
}
