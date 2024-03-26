using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(PlayerInput))]
public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;
    public float projectileSpawnOffset = 0.1f;

    private float lastFireTime = 0f;

    private PlayerInput playerInput;
    private Collider2D col;

    public void Fire(Vector2 clickPosition)
    {
        if (Time.time < lastFireTime + fireRate)
            return;

        var edge = col.ClosestPoint(clickPosition);

        var direction = (clickPosition - edge).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // our kernel/"projectile" assets are all angled 45 degrees, this corrects that:
        rotation *= Quaternion.Euler(0f, 0f, -45f);

        GameObject projectile = Instantiate(
            projectilePrefab,
            edge + direction * projectileSpawnOffset,
            rotation
        );

        projectile.GetComponent<Projectile>().Launch(direction);
        lastFireTime = Time.time;
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        playerInput.OnClickInput += Fire;
    }

    private void OnDisable()
    {
        playerInput.OnClickInput -= Fire;
    }
}
