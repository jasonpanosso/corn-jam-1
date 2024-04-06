using UnityEngine;
using UnityEngine.Events;

public class LaserEventEmitter : MonoBehaviour, ILaserTarget
{
    public UnityEvent OnHitByLaser;
    public UnityEvent LaserHitEnded;

    public void OnLaserEnter(Direction _) => OnHitByLaser.Invoke();

    public void OnLaserExit() => LaserHitEnded.Invoke();
}
