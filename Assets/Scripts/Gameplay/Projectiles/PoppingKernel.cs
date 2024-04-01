using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Collider2D))]
public class PoppingKernel : ProjectileAction
{
    public float popRadius = 2f;
    public float knockbackForce = 30f;

    [Tooltip("GameObjects with tags that will be knocked back by the Pop")]
    [SerializeField]
    private List<string> knockbackTags = new();

    [SerializeField]
    private float ignoreCollisionDuration = 0.1f;

    private Animator anim;
    private Collider2D col;

    private bool isPopped = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private System.Collections.IEnumerator SwapToPopcornLayerAfterDelay()
    {
        yield return new WaitForSeconds(ignoreCollisionDuration);

        if (gameObject != null)
            gameObject.layer = LayerMask.NameToLayer("Popcorn");
    }

    public override void Execute()
    {
        if (!isPopped)
            Pop();
    }

    private void Pop()
    {
        StartCoroutine(SwapToPopcornLayerAfterDelay());
        if (anim != null)
            anim.SetTrigger("Pop");

        var inRange = Physics2D.OverlapCircleAll(transform.position, popRadius);
        foreach (var hit in inRange)
        {
            if (knockbackTags.Any(tag => hit.CompareTag(tag)))
            {
                Vector2 knockbackDirection = (
                    hit.transform.position - transform.position
                ).normalized;

                if (knockbackDirection == Vector2.zero)
                    knockbackDirection.y = 1;

                var hitRb = hit.GetComponent<Rigidbody2D>();
                hitRb.velocity = Vector2.zero;
                hitRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }

        isPopped = true;
    }
}
