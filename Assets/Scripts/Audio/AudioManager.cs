using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : GenericSingletonMonoBehaviour<AudioManager>
{
    [SerializeField]
    private int initialAudioPoolSize = 10;

    private float _globalVolumeMod = 1f;
    public float GlobalVolumeMod
    {
        get => _globalVolumeMod;
        set => _globalVolumeMod = value;
    }

    private readonly Dictionary<string, AudioItem> audioItems = new();
    private readonly List<AudioSource> audioSourcePool = new();

    private void Awake()
    {
        InitializeAudioItemsFromResources();
        InitializeAudioPool();
    }

    public void UpdateGlobalVolume(float newVolume)
    {
        GlobalVolumeMod = newVolume;
        foreach (var source in audioSourcePool)
            if (source != null)
                source.volume *= GlobalVolumeMod;
    }

    private void StopAudioSources()
    {
        foreach (var source in audioSourcePool)
            if (source != null)
                source.Stop();
    }

    private void InitializeAudioItemsFromResources()
    {
        foreach (
            var audioAsset in Resources
                .LoadAll("Audio", typeof(AudioItemData))
                .Cast<AudioItemData>()
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

        // if all AudioSources are still playing, create a new one and add it to the pool
        GameObject audioSourceObject = new("AudioSource");
        audioSourceObject.transform.SetParent(transform);
        AudioSource newAudioSource = audioSourceObject.AddComponent<AudioSource>();
        audioSourcePool.Add(newAudioSource);
        return newAudioSource;
    }

    private void ConfigureAudioSource(AudioSource audioSource, AudioItem audioItem)
    {
        audioSource.clip = audioItem.GetRandomAudioClip();
        audioSource.volume = audioItem.volume * GlobalVolumeMod;
        audioSource.loop = audioItem.loop;
        audioSource.pitch = audioItem.GetRandomPitch();
    }

    private void OnDisable()
    {
        ServiceLocator.LevelManager.OnLevelLoadBegin -= StopAudioSources;
    }

#if UNITY_EDITOR
    protected override void OnEnable()
    {
        base.OnEnable();
        ServiceLocator.LevelManager.OnLevelLoadBegin += StopAudioSources;
        EditorCleanup();
        Awake();
    }

    private void EditorCleanup()
    {
        audioItems.Clear();
        audioSourcePool.Clear();

        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }
#else
    protected override void OnEnable()
    {
        base.OnEnable();
        ServiceLocator.LevelManager.OnLevelLoadBegin += StopAudioSources;
    }
#endif
}
