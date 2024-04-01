using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<float> OnMoveInput = delegate { };
    public event Action OnJumpDown = delegate { };
    public event Action<Vector2> OnLeftClickDown = delegate { };
    public event Action<Vector2> OnLeftClickUp = delegate { };
    public event Action<Vector2> OnRightClickDown = delegate { };

    private bool inputEnabled = true;

    private void Update()
    {
        if (!inputEnabled)
            return;

        float moveHorizontal = GetHorizontalInput();
        if (moveHorizontal != 0f)
            OnMoveInput.Invoke(moveHorizontal);

        if (GetJumpDown())
            OnJumpDown.Invoke();

        if (GetLeftClickDown())
            OnLeftClickDown.Invoke(GetCursorWorldPosition());

        if (GetLeftClickUp())
            OnLeftClickUp.Invoke(GetCursorWorldPosition());

        if (GetRightClickDown())
            OnRightClickDown.Invoke(GetCursorWorldPosition());
    }

    public void EnableInput() => inputEnabled = true;

    public void DisableInput() => inputEnabled = false;

    public float GetHorizontalInput() => Input.GetAxis("Horizontal");

    public bool GetJumpDown() => Input.GetButtonDown("Jump");

    public bool GetLeftClick() => Input.GetMouseButton(0);

    public bool GetLeftClickDown() => Input.GetMouseButtonDown(0);

    public bool GetLeftClickUp() => Input.GetMouseButtonUp(0);

    public bool GetRightClickDown() => Input.GetMouseButtonDown(1);

    public Vector2 GetCursorWorldPosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void OnEnable()
    {
        ServiceLocator.LevelManager.OnLevelComplete += DisableInput;
        ServiceLocator.LevelManager.OnLevelLoad += EnableInput;
    }

    private void OnDisable()
    {
        ServiceLocator.LevelManager.OnLevelComplete -= DisableInput;
        ServiceLocator.LevelManager.OnLevelLoad -= EnableInput;
    }
}
