using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class ProjectileShooter : MonoBehaviour
{
    [SerializeField]
    private string audioItemKey = "blop";

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float fireRate = 0.3f;

    public event Action<GameObject> OnShoot = delegate { };

    private float lastFireTime = 0f;
    private PlayerInput playerInput;

    public void Fire(Vector2 clickPosition)
    {
        if (Time.time < lastFireTime + fireRate)
            return;

        var direction = (clickPosition - (Vector2)transform.position).normalized;

        // our kernel/"projectile" assets are all angled 45 degrees, this corrects that:
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rotation *= Quaternion.Euler(0f, 0f, -45f);

        GameObject projectile = Instantiate(projectilePrefab, transform.position, rotation);
        OnShoot.Invoke(projectile);

        projectile.GetComponent<Projectile>().Launch(direction);
        lastFireTime = Time.time;

        ServiceLocator.AudioManager.PlayAudioItem(audioItemKey);
    }

    private void Awake() => playerInput = GetComponent<PlayerInput>();

    private void OnEnable() => playerInput.OnLeftClickUp += Fire;

    private void OnDisable() => playerInput.OnLeftClickUp += Fire;
}
