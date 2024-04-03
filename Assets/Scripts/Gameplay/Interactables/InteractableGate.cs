using UnityEngine;

public class InteractableGate : Interactable
{
    private enum GateState
    {
        Closed,
        Open,
    }

    [SerializeField]
    private GateState state = GateState.Closed;

    public override void Interact(GameObject _)
    {
        Debug.Log("Gate interacted with");
    }

    private void HandleStateSwitch() { }
}
