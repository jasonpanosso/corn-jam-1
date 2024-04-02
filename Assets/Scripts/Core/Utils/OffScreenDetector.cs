using System;
using UnityEngine;

public class OffScreenDetector : MonoBehaviour
{
    [SerializeField]
    private float leftBoundaryThreshhold = 0.1f;

    [SerializeField]
    private float rightBoundaryThreshhold = 0.1f;

    [SerializeField]
    private float aboveBoundaryThreshhold = 0.1f;

    [SerializeField]
    private float belowBoundaryThreshhold = 0.1f;

    [SerializeField]
    private bool detectRightOfScreen = true;

    [SerializeField]
    private bool detectLeftOfScreen = true;

    [SerializeField]
    private bool detectAboveScreen = true;

    [SerializeField]
    private bool detectBelowScreen = true;

    [SerializeField]
    private float boundaryCheckInterval = 1f;

    public event Action<GameObject> OnGameObjectOffscreen = delegate { };

    private float nextBoundaryCheckTime = 0f;

    private void Update()
    {
        if (Time.time >= nextBoundaryCheckTime)
        {
            if (IsOutsideScreenBounds())
                OnGameObjectOffscreen.Invoke(gameObject);

            nextBoundaryCheckTime = Time.time + boundaryCheckInterval;
        }
    }

    private bool IsOutsideScreenBounds()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        if (detectAboveScreen && screenPosition.y > Screen.height + aboveBoundaryThreshhold)
            return true;

        if (detectBelowScreen && screenPosition.y < -belowBoundaryThreshhold)
            return true;

        if (detectLeftOfScreen && screenPosition.x < -leftBoundaryThreshhold)
            return true;

        if (detectRightOfScreen && screenPosition.x > Screen.width + rightBoundaryThreshhold)
            return true;

        return false;
    }
}
