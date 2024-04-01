using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float jumpForce = 6f;
    public float groundAccel = 2f;

    [SerializeField]
    private ContactFilter2D groundedContactFilter;

    private Rigidbody2D rb;
    private PlayerInput playerInput;

    private bool shouldJump = false;

    // -1 = Left & 1 = Right
    private float wishDir = 0f;

    public bool IsGrounded => rb.IsTouching(groundedContactFilter);

    private void FixedUpdate()
    {
        if (wishDir != 0f)
            ApplyClampedHorizontalMovement(wishDir);

        if (shouldJump && IsGrounded)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        wishDir = 0f;
        shouldJump = false;
    }

    private void ApplyClampedHorizontalMovement(float dir)
    {
        float targetVelocityX = dir * maxSpeed;
        float velocityDifference = targetVelocityX - rb.velocity.x;

        float forceMultiplier = Mathf.Clamp01(Mathf.Abs(velocityDifference) / maxSpeed);

        Vector2 movementForce = new(velocityDifference * forceMultiplier * groundAccel, 0f);

        rb.AddForce(movementForce);

        float clampedVelocityX = Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);
        rb.velocity = new(clampedVelocityX, rb.velocity.y);
    }

    private void HandleMove(float moveInput)
    {
        wishDir = moveInput;
    }

    private void HandleJump()
    {
        shouldJump = true;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.OnMoveInput += HandleMove;
        playerInput.OnJumpDown += HandleJump;
    }

    private void OnDisable()
    {
        playerInput.OnMoveInput -= HandleMove;
        playerInput.OnJumpDown -= HandleJump;
    }
}
