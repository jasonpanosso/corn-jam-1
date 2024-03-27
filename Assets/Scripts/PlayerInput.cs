using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<float> OnMoveInput = delegate { };
    public event Action OnJumpInput = delegate { };
    public event Action<Vector2> OnClickInput = delegate { };

    private void Update()
    {
        float moveHorizontal = GetHorizontalInput();
        if (moveHorizontal != 0f)
            OnMoveInput.Invoke(moveHorizontal);

        if (GetJumpInput())
            OnJumpInput.Invoke();

        if (GetLeftClickInput())
            OnClickInput.Invoke(GetCursorWorldPosition());
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

    public Vector2 GetCursorWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
