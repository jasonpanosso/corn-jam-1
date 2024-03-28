using UnityEngine;

public class OffScreenDespawner : MonoBehaviour
{
    public float screenBoundaryThreshold = 0.1f;
    public float boundaryCheckInterval = 1f;
    private float nextBoundaryCheckTime = 0f;

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

    private bool IsOutsideScreenBounds()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return screenPosition.x < -screenBoundaryThreshold
            || screenPosition.x > Screen.width + screenBoundaryThreshold
            || screenPosition.y < -screenBoundaryThreshold
            || screenPosition.y > Screen.height + screenBoundaryThreshold;
    }
}
