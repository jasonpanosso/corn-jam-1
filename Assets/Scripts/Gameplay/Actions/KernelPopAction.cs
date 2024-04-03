using System.Collections;
using System.Collections.Generic;
using CareBoo.Serially;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class KernelPopAction : Action
{
    [TypeFilter(derivedFrom: typeof(Interactable))]
    [SerializeField]
    private List<SerializableType> interactableTypes = new();

    [SerializeField]
    private string audioKey = "SFX_PopcornPop";

    [SerializeField]
    private float popRadius = 2f;

    [SerializeField]
    private float ignoreCollisionDuration = 0.1f;

    private Animator anim;

    private bool isPopped = false;

    public override void Execute()
    {
        if (!isPopped)
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
                    Interactable target = component as Interactable;
                    target.Interact(gameObject);
                }
            }
        }

        ServiceLocator.AudioManager.PlayAudioItem(audioKey);
        isPopped = true;
    }

    private IEnumerator SwapToPopcornLayerAfterDelay()
    {
        yield return new WaitForSeconds(ignoreCollisionDuration);

        if (gameObject != null)
            gameObject.layer = LayerMask.NameToLayer("Popcorn");
    }

    private void Awake() => anim = GetComponent<Animator>();
}
