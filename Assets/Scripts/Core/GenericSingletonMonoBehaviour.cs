using UnityEngine;

[RequireComponent(typeof(SetRoot))]
[DefaultExecutionOrder(-10)]
public abstract class GenericSingletonMonoBehaviour<T> : MonoBehaviour
    where T : GenericSingletonMonoBehaviour<T>
{
    public static T Instance { get; private set; }

    protected virtual void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
