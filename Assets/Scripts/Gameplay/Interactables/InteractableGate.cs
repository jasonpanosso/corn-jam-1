using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableGate : Interactable
{
    private enum GateState
    {
        Closed,
        Open,
    }

    private Collider2D col;

    [SerializeField]
    private GateState state = GateState.Closed;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        ToggleCollisionBasedOnState();
    }

    public override void Interact(GameObject _)
    {
        FlipState();
        ToggleCollisionBasedOnState();
    }

    private void FlipState()
    {
        if (state == GateState.Closed)
            state = GateState.Open;
        else
            state = GateState.Closed;
    }

    private void ToggleCollisionBasedOnState()
    {
        if (state == GateState.Closed)
            col.enabled = true;
        else
            col.enabled = false;
    }
}
