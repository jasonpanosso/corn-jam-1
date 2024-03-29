using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PoppingKernel : ProjectileAction
{
    public float popRadius = 2f;
    public float knockbackForce = 30f;

    [Tooltip("GameObjects with tags that will be knocked back by the Pop")]
    [SerializeField]
    private List<string> knockbackTags = new();

    [SerializeField]
    private float ignoreCollisionDuration = 0.5f;

    private Animator anim;

    private bool isPopped = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private System.Collections.IEnumerator RestoreCollisionAfterDelay(Collider2D otherCollider)
    {
        yield return new WaitForSeconds(ignoreCollisionDuration);

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), otherCollider, false);
    }

    public override void Execute()
    {
        if (!isPopped)
            Pop();
    }

    private void Pop()
    {
        if (anim != null)
            anim.SetTrigger("Pop");

        var inRange = Physics2D.OverlapCircleAll(transform.position, popRadius);
        foreach (var hit in inRange)
        {
            if (knockbackTags.Any(tag => hit.CompareTag(tag)))
            {
                Physics2D.IgnoreCollision(hit, GetComponent<Collider2D>(), true);
                StartCoroutine(RestoreCollisionAfterDelay(hit));

                Vector2 knockbackDirection = (
                    hit.transform.position - transform.position
                ).normalized;

                var hitRb = hit.GetComponent<Rigidbody2D>();
                hitRb.velocity = Vector2.zero;
                hitRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }

        isPopped = true;
    }
}
