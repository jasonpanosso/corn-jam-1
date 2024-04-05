using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(IAction))]
public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    public void Launch(Vector2 direction) => rb.velocity = speed * direction.normalized;
}
