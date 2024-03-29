using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(ProjectileShooter))]
public class ProjectileManager : MonoBehaviour
{
    private readonly Queue<ProjectileAction> activeProjectileActions = new();

    private ProjectileShooter shooter;
    private PlayerInput playerInput;

    private void Awake()
    {
        shooter = GetComponent<ProjectileShooter>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void ExecuteFirstActiveProjectile(Vector2 _)
    {
        if (activeProjectileActions.Count == 0)
            return;

        // GameObjects could be despawned, which requires this jank logic.
        if (activeProjectileActions.TryDequeue(out var pa) && pa != null)
            pa.Execute();
        else
            ExecuteFirstActiveProjectile(_);
    }

    private void HandleShoot(GameObject go)
    {
        if (go.TryGetComponent<ProjectileAction>(out var pa))
            activeProjectileActions.Enqueue(pa);
        else
            Debug.LogWarning(
                "GameObject dispatched from ProjectileShooter did not have ProjectileAction component"
            );
    }

    private void OnEnable()
    {
        shooter.OnShoot += HandleShoot;
        playerInput.OnRightClickInput += ExecuteFirstActiveProjectile;
    }

    private void OnDisable()
    {
        shooter.OnShoot -= HandleShoot;
        playerInput.OnRightClickInput -= ExecuteFirstActiveProjectile;
    }
}
