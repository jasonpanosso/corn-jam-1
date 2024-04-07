using UnityEngine;

public class ZoomToTransformOnLevelComplete : MonoBehaviour
{
    [SerializeField]
    private float zoomStartTime = 0f;

    [SerializeField]
    private float zoomDuration = 1f;

    [SerializeField]
    private float zoomFactor = 2.5f;

    private void PlayZoomCutscene(int _)
    {
        CameraMovementEvent cme =
            new(transform.position, zoomStartTime, zoomDuration, zoomFactor: zoomFactor);
        ServiceLocator.CutsceneManager.PlayCutscene(new CutsceneEvent[] { cme });
    }

    private void OnEnable() => ServiceLocator.LevelManager.OnLevelComplete += PlayZoomCutscene;

    private void OnDisable() => ServiceLocator.LevelManager.OnLevelComplete -= PlayZoomCutscene;
}
