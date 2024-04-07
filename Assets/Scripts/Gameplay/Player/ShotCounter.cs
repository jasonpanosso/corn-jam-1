using UnityEngine;

public class ShotCounter : MonoBehaviour
{
    public int ShotsFired { get; private set; }

    public void Increment() => ShotsFired += 1;
}
