using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collision2D))]
public class Popper : MonoBehaviour
{
    [SerializeField]
    private LayerMask groundLayerMask = 1 << 3;

    public float popRadius = 2f;
    public float knockbackForce = 30f;

    [Tooltip("GameObjects with tags that will trigger a Pop")]
    [SerializeField]
    private List<string> triggerTags = new();

    [Tooltip("GameObjects with tags that will be knocked back by the Pop")]
    [SerializeField]
    private List<string> knockbackTags = new();

    [SerializeField]
    private float ignoreCollisionDuration = 1f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isPopped = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        ContactFilter2D filter = new();
        filter.SetLayerMask(groundLayerMask);

        Collider2D[] overlaps = new Collider2D[1];
        if (rb.OverlapCollider(filter, overlaps) > 0)
        {
            // TODO/FIXME: disgusting hack for when kernels spawn in the floor
            rb.bodyType = RigidbodyType2D.Static;
            Pop();
        }
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (isPopped)
            return;

        if (triggerTags.Any(tag => collider.gameObject.CompareTag(tag)))
            Pop();
    }

    private System.Collections.IEnumerator RestoreCollisionAfterDelay(Collider2D otherCollider)
    {
        yield return new WaitForSeconds(ignoreCollisionDuration);

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), otherCollider, false);
    }

    public void Pop()
    {
        if (anim != null)
        {
            anim.SetTrigger("Pop");
        }

        var inRange = Physics2D.OverlapCircleAll(transform.position, popRadius);
        foreach (var hit in inRange)
        {
            if (knockbackTags.Any(tag => hit.CompareTag(tag)))
            {
                Physics2D.IgnoreCollision(hit, GetComponent<Collider2D>(), true);

                // StartCoroutine(RestoreCollisionAfterDelay(hit));

                Vector2 knockbackDirection = (
                    hit.transform.position - transform.position
                ).normalized;

                // Debug.DrawLine(hit.transform.position, transform.position, Color.red, 3);
                // Debug.Log(hit.attachedRigidbody.name);
                // Debug.Log($"hit transform: {hit.transform.position}");
                // Debug.Log($"popper transform: {transform.position}");
                // Debug.Log(
                //     $"normalized: {(hit.transform.position - transform.position).normalized}"
                // );

                var hitRb = hit.GetComponent<Rigidbody2D>();
                hitRb.velocity = Vector2.zero;
                hitRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }

        isPopped = true;
    }
}
