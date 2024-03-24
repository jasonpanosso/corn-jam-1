using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 3f;

    [SerializeField]
    private ContactFilter2D ContactFilter;

    private Rigidbody2D rb;
    private PlayerInput playerInput;

    public bool IsGrounded => rb.IsTouching(ContactFilter);

    private void HandleMove(float moveInput)
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void HandleJump()
    {
        if (!IsGrounded)
            return;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.OnMove += HandleMove;
        playerInput.OnJump += HandleJump;
    }

    private void OnDisable()
    {
        playerInput.OnMove -= HandleMove;
        playerInput.OnJump -= HandleJump;
    }
}
