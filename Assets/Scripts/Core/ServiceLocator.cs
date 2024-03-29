public static class ServiceLocator
{
    public static AudioManager AudioManager => AudioManager.Instance;
    public static LevelManager LevelManager => LevelManager.Instance;
}
