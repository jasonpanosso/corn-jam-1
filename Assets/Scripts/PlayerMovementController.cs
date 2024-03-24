using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 6f;

    [SerializeField]
    private LayerMask groundLayer = 1 << 3;
    private ContactFilter2D GroundedContactFilter = new();

    private Rigidbody2D rb;
    private PlayerInput playerInput;

    public bool IsGrounded => rb.IsTouching(GroundedContactFilter);

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

        GroundedContactFilter.SetLayerMask(groundLayer);
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
