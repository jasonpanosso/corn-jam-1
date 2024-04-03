using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableGate : Interactable
{
    private enum GateState
    {
        Closed,
        Open,
    }

    [SerializeField]
    private GateState state = GateState.Closed;

    [SerializeField]
    private float moveDuration = 1f;

    private Collider2D col;
    private GameObject lowerGate;
    private GameObject middleGate;
    private GameObject upperGate;

    private void Awake()
    {
        InitializeGatePieces();

        col = GetComponent<Collider2D>();
        if (state == GateState.Closed)
            col.enabled = true;
        else
            col.enabled = false;
    }

    public override void Interact(GameObject _)
    {
        StopAllCoroutines();

        if (state == GateState.Closed)
        {
            state = GateState.Open;
            col.enabled = false;
            RaiseGate();
        }
        else
        {
            state = GateState.Closed;
            col.enabled = true;
            LowerGate();
        }
    }

    private void LowerGate()
    {
        var upperY = upperGate.transform.position.y;

        var middleTarget = upperY - 1f;
        var lowerTarget = upperY - 2f;

        StartCoroutine(MoveCoroutine(middleGate, middleTarget, moveDuration));
        StartCoroutine(MoveCoroutine(lowerGate, lowerTarget, moveDuration));
    }

    private void RaiseGate()
    {
        var upperY = upperGate.transform.position.y;

        StartCoroutine(MoveCoroutine(middleGate, upperY, moveDuration));
        StartCoroutine(MoveCoroutine(lowerGate, upperY, moveDuration));
    }

    private IEnumerator MoveCoroutine(GameObject go, float endY, float duration)
    {
        var startPos = go.transform.position;
        var endPos = upperGate.transform.position;
        endPos.y = endY;

        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            go.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        go.transform.position = endPos;
    }

    private void InitializeGatePieces()
    {
        var gatePieces = GetComponentsInChildren<SpriteRenderer>()
            .Select(sr => sr.gameObject)
            .ToList();

        if (gatePieces.Count != 3)
            Debug.LogWarning("Unimplemented: Gates with more than three gate pieces/cubes");

        gatePieces.Sort((a, b) => a.transform.position.y < b.transform.position.y ? 1 : 0);

        upperGate = gatePieces[0];
        middleGate = gatePieces[1];
        lowerGate = gatePieces[2];

        if (state == GateState.Open)
        {
            middleGate.transform.position = upperGate.transform.position;
            lowerGate.transform.position = upperGate.transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        GUIStyle style = new();
        style.normal.textColor = Color.white;
        style.fontSize = 10;

        Vector3 position = transform.position;
        position.y -= 3.5f;
        position.x -= 2.0f;

        string label = $"Gate: {state}";

        Handles.Label(position, label, style);
    }
}
