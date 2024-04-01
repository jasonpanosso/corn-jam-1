# Audio Documentation

Audio in the game is broken down like so:

`AudioManager` is automatically instantiated into each scene via a custom level
prefab configured in `Assets/LDtkData/CornLevelsLDtk.ldtk`

the singleton `AudioManager` manages the lifecycle of [AudioSources](https://docs.unity3d.com/ScriptReference/AudioSource.html)

To play an audio file, make a call to `ServiceLocator.AudioManager.PlayAudioItem(NAME_OF_AUDIO_ITEM);`

## Adding new Audio Files

To add a new audio file and have `AudioManager` know about it, follow these steps:

1. Add the raw audio file(`.wav`, `.mp3`, etc) to `Assets/Audio`.
2. To add a new `AudioItemScriptableObject` right click on the `Assets/Resources/Audio` directory
   and select `Create > Audio > AudioItem`.
3. Once created, drag the relevant audio file(s) into the `AudioItemScriptableObject`'s
   list of audio clips, and configure the volume/pitch as you please.
4. Within a script, call `ServiceLocator.AudioManager.PlayAudioItem(NAME_OF_AUDIO_ITEM_SCRIPTABLE_OBJECT_FILE);`
