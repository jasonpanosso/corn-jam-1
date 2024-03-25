using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;
    public float projectileSpawnOffset = 0.7f;
    private float lastFireTime = 0f;

    private PlayerInput playerInput;

    public void Fire(Vector2 _clickPosition)
    {
        if (Time.time >= lastFireTime + fireRate)
        {
            GameObject projectile = Instantiate(
                projectilePrefab,
                transform.position,
                Quaternion.identity
            );
            projectile.GetComponent<Projectile>().Launch(transform.up);

            lastFireTime = Time.time;
        }
    }

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("Weapon component instantiated without PlayerInput component in parent");
        }
    }

    private void Update()
    {
        UpdateWeaponPosition();
    }

    // NOTE: we are calculating the weapons position every frame based on the
    // player's(parent's) transform for future-proofing the case where we want
    // to have an aiming indicator.
    private void UpdateWeaponPosition()
    {
        Vector2 cursorPosition = playerInput.GetCursorPosition();
        Vector2 playerPosition = transform.parent.position;
        Vector2 direction = (cursorPosition - playerPosition).normalized;

        transform.SetPositionAndRotation(
            playerPosition + direction * projectileSpawnOffset,
            Quaternion.LookRotation(Vector3.forward, direction)
        );
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
