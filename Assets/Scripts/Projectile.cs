using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float screenBoundaryThreshold = 0.1f;
    public float boundaryCheckInterval = 1f;
    private float nextBoundaryCheckTime = 0f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        nextBoundaryCheckTime = Time.time + boundaryCheckInterval;
    }

    private void Update()
    {
        if (Time.time >= nextBoundaryCheckTime)
        {
            if (IsOutsideScreenBounds())
            {
                Destroy(gameObject);
            }

            nextBoundaryCheckTime = Time.time + boundaryCheckInterval;
        }
    }

    public void Launch(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
    }

    private bool IsOutsideScreenBounds()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return screenPosition.x < -screenBoundaryThreshold
            || screenPosition.x > Screen.width + screenBoundaryThreshold
            || screenPosition.y < -screenBoundaryThreshold
            || screenPosition.y > Screen.height + screenBoundaryThreshold;
    }
}
