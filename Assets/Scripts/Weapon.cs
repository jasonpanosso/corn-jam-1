using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;
    public float projectileSpawnOffset = 2f;

    private PlayerInput playerInput;

    private float lastFireTime = 0f;

    public void Fire(Vector2 clickPosition)
    {
        if (Time.time >= lastFireTime + fireRate)
        {
            Vector2 aimDirection = (clickPosition - (Vector2)transform.position).normalized;
            Vector2 spawnPoint = (Vector2)transform.position + aimDirection * projectileSpawnOffset;

            GameObject projectile = Instantiate(projectilePrefab, spawnPoint, Quaternion.identity);
            projectile.GetComponent<Projectile>().Launch(aimDirection);

            lastFireTime = Time.time;
        }
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
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
