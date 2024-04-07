using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : MonoBehaviour, IInteractable
{
    public Direction LaserDirection;

    [SerializeField]
    private float laserMaxLength = 50f;

    [SerializeField]
    private bool _isEmitting = true;

    public bool IsEmitting
    {
        get => _isEmitting;
        set
        {
            lr.SetPositions(new Vector3[] { transform.position, transform.position });
            lr.enabled = value;
            _isEmitting = value;

            if (!target.IsUnityNull())
                target.OnLaserExit();

            target = null;
        }
    }

    private LineRenderer lr;

    private readonly Vector3[] lrPositions = new Vector3[2];

    private ILaserTarget target;

    private void Update()
    {
        if (!IsEmitting)
            return;

        var dir = directionToVecMap[LaserDirection];
        var hit = Physics2D.Raycast(transform.position, dir);

        var endPoint =
            hit.collider != null ? hit.point : ((Vector2)transform.position + dir * laserMaxLength);

        lrPositions[0] = transform.position;
        lrPositions[1] = endPoint;

        lr.SetPositions(lrPositions);

        if (hit.collider != null && hit.collider.TryGetComponent<ILaserTarget>(out var laserTarget))
        {
            if (laserTarget == target)
                return;

            if (!target.IsUnityNull())
                target.OnLaserExit();

            laserTarget.OnLaserEnter(inverseDirectionMap[LaserDirection]);
            target = laserTarget;
        }
        else if (!target.IsUnityNull())
        {
            target.OnLaserExit();
            target = null;
        }
    }

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = IsEmitting;
    }

    public void Interact(GameObject _) => IsEmitting = !IsEmitting;

    private readonly Dictionary<Direction, Vector2> directionToVecMap =
        new()
        {
            { Direction.North, Vector2.up },
            { Direction.South, Vector2.down },
            { Direction.West, Vector2.left },
            { Direction.East, Vector2.right },
        };

    private readonly Dictionary<Direction, Direction> inverseDirectionMap =
        new()
        {
            { Direction.North, Direction.South },
            { Direction.South, Direction.North },
            { Direction.West, Direction.East },
            { Direction.East, Direction.West },
        };
}
