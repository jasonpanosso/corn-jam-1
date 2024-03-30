using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<float> OnMoveInput = delegate { };
    public event Action OnJumpInput = delegate { };
    public event Action<Vector2> OnLeftClickInput = delegate { };
    public event Action<Vector2> OnRightClickInput = delegate { };

    private bool inputDisabled = false;

    private void Update()
    {
        if (inputDisabled)
            return;

        float moveHorizontal = GetHorizontalInput();
        if (moveHorizontal != 0f)
            OnMoveInput.Invoke(moveHorizontal);

        if (GetJumpInput())
            OnJumpInput.Invoke();

        if (GetLeftClickInput())
            OnLeftClickInput.Invoke(GetCursorWorldPosition());

        if (GetRightClickInput())
            OnRightClickInput.Invoke(GetCursorWorldPosition());
    }

    public void EnableInput()
    {
        inputDisabled = false;
    }

    public void DisableInput()
    {
        inputDisabled = true;
    }

    public float GetHorizontalInput()
    {
        return Input.GetAxis("Horizontal");
    }

    public bool GetJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public bool GetLeftClickInput()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool GetRightClickInput()
    {
        return Input.GetMouseButtonDown(1);
    }

    public Vector2 GetCursorWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
