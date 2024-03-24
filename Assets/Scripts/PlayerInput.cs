using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<float> OnMove = delegate { };
    public event Action OnJump = delegate { };

    private void Update()
    {
        OnMove.Invoke(Input.GetAxis("Horizontal"));

        if (Input.GetButtonDown("Jump"))
        {
            OnJump.Invoke();
        }
    }

    public float GetHorizontalInput()
    {
        return Input.GetAxis("Horizontal");
    }

    public bool GetJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }
}
