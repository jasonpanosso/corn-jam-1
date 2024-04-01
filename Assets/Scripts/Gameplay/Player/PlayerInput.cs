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

    public void EnableInput() => inputDisabled = false;

    public void DisableInput() => inputDisabled = true;

    public float GetHorizontalInput() => Input.GetAxis("Horizontal");

    public bool GetJumpInput() => Input.GetButtonDown("Jump");

    public bool GetLeftClickInput() => Input.GetMouseButtonDown(0);

    public bool GetRightClickInput() => Input.GetMouseButtonDown(1);

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
