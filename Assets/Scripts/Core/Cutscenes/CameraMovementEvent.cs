using Cinemachine;
using UnityEngine;

[System.Serializable]
public class CameraMovementEvent : CutsceneEvent
{
    public Vector2 targetPosition;
    public float zoomFactor;

    private CinemachineVirtualCamera virtualCamera;
    private Vector3 startPosition;
    private float elapsedTime = 0f;
    private float startOrthoSize;
    private float goalOrthoSize;

    public override void Init()
    {
        virtualCamera = Object.FindObjectOfType<CinemachineVirtualCamera>();
        startPosition = virtualCamera.transform.position;

        startOrthoSize = virtualCamera.m_Lens.OrthographicSize;
        goalOrthoSize = startOrthoSize / zoomFactor;
    }

    public override void Update(float deltaTime)
    {
        if (virtualCamera == null)
        {
            elapsedTime = duration;
            return;
        }

        elapsedTime += deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);

        virtualCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startOrthoSize, goalOrthoSize, t);
    }

    public override bool IsFinished()
    {
        return elapsedTime >= duration;
    }

    public CameraMovementEvent(
        Vector2 targetPosition,
        float startTime,
        float duration,
        float zoomFactor = 1
    )
    {
        this.targetPosition = targetPosition;
        this.zoomFactor = zoomFactor;
        this.startTime = startTime;
        this.duration = duration;
    }
}
