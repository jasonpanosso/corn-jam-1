using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<float> OnMoveInput = delegate { };
    public event Action OnJumpInput = delegate { };
    public event Action<Vector2> OnClickInput = delegate { };


    private void Update()
    {
        // TODO: this is clunky; it just fires every frame until we figure out
        // how movement should work. Not setting up something intelligent rn.
        float moveHorizontal = GetHorizontalInput();
        OnMoveInput.Invoke(moveHorizontal);

        if (GetJumpInput())
            OnJumpInput.Invoke();

        if (GetLeftClickInput())
            OnClickInput.Invoke(GetCursorPosition());

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

    public Vector2 GetCursorPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
