using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockbackInteractable : Interactable
{
    [SerializeField]
    private float knockbackForce = 30f;

    private Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    public override void Interact(Action actor)
    {
        Vector2 knockbackDirection = (transform.position - actor.transform.position).normalized;

        // If popped directly in center of player/a transform, just send them up.
        if (knockbackDirection == Vector2.zero)
            knockbackDirection.y = 1;

        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }
}
