using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float lifetime = 3f;
    private GameObject player;
    [SerializeField] private int daño = 1;

    private Rigidbody2D rb;

    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start ()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection ( Vector2 dir, float spd )
    {
        direction = dir.normalized;
        speed = spd;

        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().RecibirDaño(daño);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
