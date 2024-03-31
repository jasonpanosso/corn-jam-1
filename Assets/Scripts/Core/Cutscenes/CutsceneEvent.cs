[System.Serializable]
public abstract class CutsceneEvent
{
    public float startTime;
    public float duration;

    public abstract void Init();
    public abstract void Update(float deltaTime);
    public abstract bool IsFinished();
}
