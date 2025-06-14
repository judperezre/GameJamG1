using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
    [Header("Nivel")]
    public static int nivel = 1;

    [Header("Movimiento")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Salto Mejorado")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Disparo")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireCooldown;
    [SerializeField] private float offsetBullet = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip audioJump;
    [SerializeField] private AudioClip audioDisparo;
    [SerializeField] private AudioClip audioPoweup;
    [SerializeField] private AudioClip audioDa�o;

    [Header("Limite de Pantalla")]
    [SerializeField] private float minX, maxX, minY, maxY;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] public GameObject[] bulletPrefab;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalInput;
    private bool facingRight = true;
    private float fireCooldownTimer = 0f;

    public static PlayerController Instance;

    void Awake ()
    {
        Instance = this;

        // Siempre actualizamos el target de la c�mara al nuevo jugador
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        if (camera != null)
        {
            camera.player = this.transform;
        }
    }

    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        respawnPoint = transform;
    }

    void Update ()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        Mover();
        Saltar();
        MejorarSalto();
        Disparar();
        Pausar();
        fireCooldownTimer -= Time.deltaTime;
    }

    void LateUpdate ()
    {
        LimitarPosicion();
    }

    void LimitarPosicion ()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(transform.position.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = clampedPosition;
    }

    void Mover ()
    {
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        if (horizontalInput > 0)
            facingRight = true;
        else if (horizontalInput < 0)
            facingRight = false;
    }

    void Pausar ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.PauseGame();
        }
    }

    void Saltar ()
    {
        if (nivel == 1)
        {
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                Debug.Log("Salto detectado");
                audioSource.PlayOneShot(audioJump);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                Debug.Log("Salto realizado");
            }
        }
        if (nivel >= 2)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                audioSource.PlayOneShot(audioJump);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                Debug.Log("Salto realizado");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                audioSource.PlayOneShot(audioJump);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                Debug.Log("Salto realizado");
            }
        }
    }

    void MejorarSalto ()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void Disparar ()
    {
        Vector3 offset = facingRight ? Vector3.right * offsetBullet : Vector3.left * offsetBullet;

        if (Input.GetKeyDown(KeyCode.Space) && fireCooldownTimer <= 0f)
        {
            audioSource.PlayOneShot(audioDisparo);
            GameObject bullet;
            BulletScript bulletScript;

            int bulletIndex = nivel >= 2 ? 1 : 0;

            bullet = Instantiate(bulletPrefab[bulletIndex], firePoint.position + offset, Quaternion.identity);
            bulletScript = bullet.GetComponent<BulletScript>();
            bulletScript.SetDirection(facingRight ? Vector2.right : Vector2.left, bulletSpeed);
            bulletScript.SetScreenLimits(minX, maxX, minY, maxY);
        }
    }

    private void OnCollisionEnter2D ( Collision2D collision )
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D ( Collision2D collision )
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D ( Collider2D other )
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            audioSource.PlayOneShot(audioPoweup);
            Destroy(other.gameObject);
            GameManager.Instance.RestaurarVida();
            if (nivel != 2)
            {
                nivel++;
                GameManager.Instance.RestaurarVida();
                CambiarPrefab();
            }
        }
        else if (other.gameObject.CompareTag("ObstacleDamage"))
        {
            GameManager.Instance.RestarVida(1);
        }
    }

    void CambiarPrefab ()
    {
        Vector3 posicionActual = transform.position;
        Quaternion rotacionActual = transform.rotation;

        gameObject.SetActive(false);

        GameObject nuevoPlayer = Instantiate(playerPrefabs[nivel - 1], posicionActual, rotacionActual);
        Destroy(gameObject);
    }
}
