using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableTimerGate : Interactable
{
    private enum GateState
    {
        Closed,
        Open,
    }

    private GateState state = GateState.Closed;

    [SerializeField]
    private float moveDuration = 1f;

    [SerializeField]
    private float timerLength = 10f;

    private float curTimer = 0f;

    private Collider2D col;
    private GameObject lowerGate;
    private GameObject middleGate;
    private GameObject upperGate;

    private void Awake()
    {
        InitializeGatePieces();

        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (state == GateState.Closed)
            return;

        curTimer -= Time.deltaTime;

        if (curTimer <= 0f)
        {
            StopAllCoroutines();
            state = GateState.Closed;
            col.enabled = true;
            LowerGate();
        }
    }

    public override void Interact(GameObject _)
    {
        StopAllCoroutines();

        if (state == GateState.Closed)
            RaiseGate();

        curTimer = timerLength;
        state = GateState.Open;
        col.enabled = false;
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
        var gatePieces = new List<GameObject>();
        foreach (Transform child in transform)
            gatePieces.Add(child.gameObject);

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
}
