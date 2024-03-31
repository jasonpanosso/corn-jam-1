using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private int initialAudioPoolSize = 10;

    private readonly Dictionary<string, AudioItem> audioItems = new();
    private readonly List<AudioSource> audioSourcePool = new();

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

#if UNITY_EDITOR
        EditorCleanup();
        Awake();
#endif
    }

    private void Awake()
    {
        InitializeAudioItemsFromResources();
        InitializeAudioPool();
    }

    private void InitializeAudioItemsFromResources()
    {
        foreach (
            var audioAsset in Resources
                .LoadAll("Audio", typeof(AudioItemScriptableObject))
                .Cast<AudioItemScriptableObject>()
        )
            RegisterAudioItem(audioAsset.name, audioAsset.ToAudioItem());
    }

    private void InitializeAudioPool()
    {
        for (int i = 0; i < initialAudioPoolSize; i++)
        {
            GameObject audioSourceObject = new("AudioSource");
            audioSourceObject.transform.SetParent(transform);
            AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
            audioSourcePool.Add(audioSource);
        }
    }

    public void RegisterAudioItem(string key, AudioItem audioItem)
    {
        if (!audioItems.ContainsKey(key))
            audioItems.Add(key, audioItem);
        else
            Debug.LogWarning($"AudioItem with key '{key}' already exists.");
    }

    public void PlayAudioItem(string key)
    {
        if (audioItems.TryGetValue(key, out AudioItem audioItem))
        {
            var audioSource = GetAudioSourceFromPool();
            ConfigureAudioSource(audioSource, audioItem);
            audioSource.Play();
        }
        else
            Debug.LogWarning($"AudioItem with key '{key}' not found.");
    }

    private AudioSource GetAudioSourceFromPool()
    {
        foreach (AudioSource audioSource in audioSourcePool)
            if (!audioSource.isPlaying)
                return audioSource;

        // if no all AudioSources are still playing, create a new one and add it to the pool
        GameObject audioSourceObject = new("AudioSource");
        audioSourceObject.transform.SetParent(transform);
        AudioSource newAudioSource = audioSourceObject.AddComponent<AudioSource>();
        audioSourcePool.Add(newAudioSource);
        return newAudioSource;
    }

    private void ConfigureAudioSource(AudioSource audioSource, AudioItem audioItem)
    {
        audioSource.clip = audioItem.GetRandomAudioClip();
        audioSource.volume = audioItem.volume;
        // audioSource.pitch = audioItem.GetRandomPitch();
    }

#if UNITY_EDITOR
    private void EditorCleanup()
    {
        audioItems.Clear();
        audioSourcePool.Clear();

        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }
#endif
}
