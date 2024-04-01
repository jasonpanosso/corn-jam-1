# Levels

## LDtk

We use [LDtk](https://ldtk.io/) to create levels. We then use [LDtkToUnity](https://cammin.github.io/LDtkToUnity/)
to import the `.ldtk` files and automagically create prefabs for each level.
These prefabs will have colliders automatically setup for them.

If you are designing levels, please speak to Jason on how to setup LDtkToUnity
to automatically import your LDtk levels into Unity, or read the LDtkToUnity
setup documentation.

## General Level design/implementation workflow

1. Design a level in LDtk
2. Save the level, which will fire the LDtkToUnity import process.
3. Navigate to `Assets/Scenes/Levels`. There are directories for each world in
   LDtk, navigate to the corresponding one, and then open the scene which has the
   same name as the level you created
4. Drag the corresponding LDtk level prefab from the `Assets/LDtkData/CornLevelsLDtk`
   directory into this scene, add the camera prefab, and customize the level in
   Unity as you see fit

## LevelManager

Levels are managed via a `LevelManager`

`LevelManager` is automatically instantiated into each scene that has an LDtk level
prefab via a custom level prefab configured in `Assets/LDtkData/CornLevelsLDtk.ldtk`

`LevelManager` contains a list of `Level` objects which contains information
about each level, including the corresponding scene that should be loaded, the
level number(index), and the "formatted"(user-facing) name.

`LevelManager` initializes this list by by referencing `Assets/Resources/AllLevelData`
at runtime. For information regarding `AllLevelData`, see the Custom LDtkToUnity
PostProcessor editor script section.

This list is used to load levels by index. The list is sorted by the order that
the levels should be played in, therefore the first item in the list will be
the first level of the game, the second item would be the second level, etc.

To load a level, call `ServiceLocator.LevelManager.LoadLevel(LEVEL_INDEX)`

## Custom `LDtkToUnity` PostProcessor editor script

This script has a handful of jobs:

1. Managing the `AllLevelsData` asset. `AllLevelData` is a `ScriptableObject` that contains
   all instances of `Level`. This ScriptableObject is automatically created, and managed via an
   editor script whenever our LDtkToUnity importer detects changes to levels.
2. Creating new scenes for new LDtk levels, and adding said scene to the list
   of scenes that Unity will include at buildtime.
