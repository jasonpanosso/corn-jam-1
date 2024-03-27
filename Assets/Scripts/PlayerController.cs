using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 6f;
    public float groundAccel = 2f;

    [SerializeField]
    private LayerMask groundLayer = 1 << 3;
    private ContactFilter2D groundedContactFilter = new();

    private Rigidbody2D rb;
    private PlayerInput playerInput;

    public bool IsGrounded => rb.IsTouching(groundedContactFilter);

    private void HandleMove(float moveInput)
    {
        // Remove air control
        if (!IsGrounded)
            return;

        float targetVelocityX = moveInput * moveSpeed;
        float forceMultiplier = Mathf.Clamp01(
            (Mathf.Abs(targetVelocityX) - Mathf.Abs(rb.velocity.x)) * groundAccel
        );
        Vector2 movementForce = new((targetVelocityX - rb.velocity.x) * forceMultiplier, 0f);
        rb.AddForce(movementForce);

        float clampedVelocityX = Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed);
        rb.velocity = new Vector2(clampedVelocityX, rb.velocity.y);
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

        groundedContactFilter.SetLayerMask(groundLayer);
    }

    private void OnEnable()
    {
        playerInput.OnMoveInput += HandleMove;
        playerInput.OnJumpInput += HandleJump;
    }

    private void OnDisable()
    {
        playerInput.OnMoveInput -= HandleMove;
        playerInput.OnJumpInput -= HandleJump;
    }
}
