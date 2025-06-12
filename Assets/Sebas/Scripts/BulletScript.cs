using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float lifetime = 3f;
    private GameObject gameManager;
    [SerializeField] private int daño = 1;
    [SerializeField] private int puntos = 10;

    private Rigidbody2D rb;

    // NUEVO: Límites de pantalla
    private float minX, maxX, minY, maxY;

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

    // NUEVO: Recibir los límites desde el PlayerController
    public void SetScreenLimits ( float minX, float maxX, float minY, float maxY )
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
    }

    void Update ()
    {
        // Chequea cada frame si está fuera de la pantalla
        if (transform.position.x < minX || transform.position.x > maxX ||
            transform.position.y < minY || transform.position.y > maxY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            collision.GetComponent<Enemy>().RecibirDaño(daño);
            GameManager.Instance.SumarPuntaje(puntos);
            Debug.Log("Enemigo golpeado, puntos sumados: " + puntos);
            Debug.Log("Enemigo golpeado, daño infligido: " + daño);
        }
        else if (collision.CompareTag("Player"))
        {
            GameManager.Instance.RestarVida(daño);
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
