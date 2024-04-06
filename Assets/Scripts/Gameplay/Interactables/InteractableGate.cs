using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class InteractableGate : MonoBehaviour, IInteractable
{
    private enum GateState
    {
        Closed,
        Open,
    }

    public UnityEvent OnGateOpen;
    public UnityEvent OnGateClose;

    [SerializeField]
    private GateState state = GateState.Closed;

    [SerializeField]
    private float moveDuration = 1f;

    [SerializeField]
    private GameObject lowerGate;

    [SerializeField]
    private GameObject middleGate;

    [SerializeField]
    private GameObject upperGate;

    private Vector3 lowerStartPos;
    private Vector3 middleStartPos;

    private void Awake()
    {
        InitializeGatePieces();
    }

    public void Interact(GameObject _)
    {
        StopAllCoroutines();

        if (state == GateState.Closed)
        {
            state = GateState.Open;
            RaiseGate();
        }
        else
        {
            state = GateState.Closed;
            LowerGate();
        }
    }

    private void LowerGate()
    {
        OnGateClose.Invoke();
        StartCoroutine(MoveCoroutine(middleGate, middleStartPos, moveDuration));
        StartCoroutine(MoveCoroutine(lowerGate, lowerStartPos, moveDuration));
    }

    private void RaiseGate()
    {
        OnGateOpen.Invoke();
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
        position.x += 0.3f;

        string label = $"Gate: {state}";

        Handles.Label(position, label, style);
    }
#endif
}
