using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(LaserEmitter))]
public class InteractableMirror : MonoBehaviour, IInteractable, ILaserTarget
{
    private enum MirrorOrientation
    {
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest
    }

    [SerializeField]
    private MirrorOrientation orientation;

    private LaserEmitter emitter;

    private Direction? inputDirection = null;

    public void Interact(GameObject _) => RotateClockwise();

    public void OnLaserEnter(Direction inDir) => ReflectLaser(inDir);

    private void ReflectLaser(Direction inDir)
    {
        inputDirection = inDir;

        if (reflectionMap.TryGetValue((orientation, inDir), out var reflectedDir))
        {
            emitter.LaserDirection = reflectedDir;
            emitter.enabled = true;
        }
        else
            emitter.enabled = false;
    }

    public void OnLaserExit()
    {
        inputDirection = null;

        if (emitter != null)
            emitter.enabled = false;
    }

    private void RotateClockwise()
    {
        if (orientation == MirrorOrientation.NorthEast)
            orientation = MirrorOrientation.SouthEast;
        else if (orientation == MirrorOrientation.SouthEast)
            orientation = MirrorOrientation.SouthWest;
        else if (orientation == MirrorOrientation.SouthWest)
            orientation = MirrorOrientation.NorthWest;
        else if (orientation == MirrorOrientation.NorthWest)
            orientation = MirrorOrientation.NorthEast;

        if (inputDirection != null)
            ReflectLaser((Direction)inputDirection);

        transform.Rotate(0, 0, -90f);
    }

    private void Awake()
    {
        emitter = GetComponent<LaserEmitter>();
        emitter.enabled = false;
        transform.Rotate(0, 0, rotationMap[orientation]);
    }

    private readonly Dictionary<(MirrorOrientation, Direction), Direction> reflectionMap =
        new()
        {
            { (MirrorOrientation.NorthEast, Direction.North), Direction.East },
            { (MirrorOrientation.NorthEast, Direction.East), Direction.North },
            { (MirrorOrientation.NorthWest, Direction.North), Direction.West },
            { (MirrorOrientation.NorthWest, Direction.West), Direction.North },
            { (MirrorOrientation.SouthEast, Direction.South), Direction.East },
            { (MirrorOrientation.SouthEast, Direction.East), Direction.South },
            { (MirrorOrientation.SouthWest, Direction.South), Direction.West },
            { (MirrorOrientation.SouthWest, Direction.West), Direction.South }
        };

    private readonly Dictionary<MirrorOrientation, float> rotationMap =
        new()
        {
            { MirrorOrientation.NorthEast, 0f },
            { MirrorOrientation.SouthEast, -90f },
            { MirrorOrientation.SouthWest, 180f },
            { MirrorOrientation.NorthWest, 90f }
        };
}
