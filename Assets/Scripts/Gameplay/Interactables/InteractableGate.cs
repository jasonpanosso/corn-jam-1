using System.Collections;
using System.Collections.Generic;
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

    private Vector3 lowerStartPos;
    private Vector3 middleStartPos;

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
        StartCoroutine(MoveCoroutine(middleGate, middleStartPos, moveDuration));
        StartCoroutine(MoveCoroutine(lowerGate, lowerStartPos, moveDuration));
    }

    private void RaiseGate()
    {
        StartCoroutine(MoveCoroutine(middleGate, upperGate.transform.position, moveDuration));
        StartCoroutine(MoveCoroutine(lowerGate, upperGate.transform.position, moveDuration));
    }

    private IEnumerator MoveCoroutine(GameObject go, Vector2 endPos, float duration)
    {
        var startPos = go.transform.position;
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
        var gatePieces = new List<GameObject>();
        foreach (Transform child in transform)
            gatePieces.Add(child.gameObject);

        if (gatePieces.Count != 3)
            Debug.LogWarning("Unimplemented: Gates with more than three gate pieces/cubes");

        gatePieces.Sort((a, b) => a.transform.position.y < b.transform.position.y ? 1 : 0);

        upperGate = gatePieces[0];
        middleGate = gatePieces[1];
        lowerGate = gatePieces[2];

        middleStartPos = middleGate.transform.position;
        lowerStartPos = lowerGate.transform.position;

        if (state == GateState.Open)
        {
            middleGate.transform.position = upperGate.transform.position;
            lowerGate.transform.position = upperGate.transform.position;
        }
    }

#if UNITY_EDITOR
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
#endif
}
