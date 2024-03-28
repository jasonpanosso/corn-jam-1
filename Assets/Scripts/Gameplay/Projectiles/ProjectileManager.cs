using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(ProjectileShooter))]
public class ProjectileManager : MonoBehaviour
{
    private readonly Queue<ProjectileAction> activeProjectileActions = new();

    public void ExecuteFirstActiveProjectile(Vector2 _)
    {
        if (activeProjectileActions.TryDequeue(out var pa))
            pa.Execute();
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
        var shooter = FindObjectOfType<ProjectileShooter>();
        if (shooter)
            shooter.OnShoot += HandleShoot;

        var pi = FindObjectOfType<PlayerInput>();
        if (pi)
            pi.OnRightClickInput += ExecuteFirstActiveProjectile;
    }

    private void OnDisable()
    {
        var shooter = FindObjectOfType<ProjectileShooter>();
        if (shooter)
            shooter.OnShoot -= HandleShoot;

        var pi = FindObjectOfType<PlayerInput>();
        if (pi)
            pi.OnRightClickInput -= ExecuteFirstActiveProjectile;
    }
}
