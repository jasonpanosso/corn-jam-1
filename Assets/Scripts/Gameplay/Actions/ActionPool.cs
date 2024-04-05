using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// HACK/FIXME: This is mega-coupled with player input and projectileshooter.
// VERY sus. Not worth fixing until it is problematic IMO.
[RequireComponent(typeof(PlayerInput), typeof(ProjectileShooter))]
public class ActionPool : MonoBehaviour
{
    private readonly Queue<IAction> activeActions = new();

    private ProjectileShooter shooter;
    private PlayerInput playerInput;

    private void Awake()
    {
        shooter = GetComponent<ProjectileShooter>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void ExecuteFirstActiveAction(Vector2 _)
    {
        if (activeActions.Count == 0)
            return;

        // GameObjects could be despawned, which requires this jank logic.
        if (activeActions.TryDequeue(out var action) && !action.IsUnityNull())
            action.Execute();
        else
            // retry if GO is null until valid action is found or queue is empty
            ExecuteFirstActiveAction(_);
    }

    private void EnqueueAction(GameObject go)
    {
        if (go.TryGetComponent<IAction>(out var action))
            activeActions.Enqueue(action);
        else
            Debug.LogWarning("GameObject added to ActionPool did not have Action component");
    }

    private void OnEnable()
    {
        shooter.OnShoot += EnqueueAction;
        playerInput.OnRightClickDown += ExecuteFirstActiveAction;
    }

    private void OnDisable()
    {
        shooter.OnShoot -= EnqueueAction;
        playerInput.OnRightClickDown -= ExecuteFirstActiveAction;
    }
}
