using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [Header("Nivel")]
    public static int nivel = 1;

    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Salto Mejorado")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Disparo")]
    [SerializeField] public GameObject[] bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireCooldown;
    [SerializeField] private float offsetBullet = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip audioJump;
    [SerializeField] private AudioClip audioDisparo;
    [SerializeField] private AudioClip audioPoweup;
    [SerializeField] private AudioClip audioDaño;

    [Header("Limite de Pantalla")]
    [SerializeField] private float minX, maxX, minY, maxY;

    [Header("Powerup")]
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject powerup;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalInput;
    private bool facingRight = true;
    private float fireCooldownTimer = 0f;

    void Start ()
    {
        powerup = GameObject.Find("Powerup");
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;


        respawnPoint = transform; // para mantener la posición actual
    }

    void Update ()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        Mover();
        Saltar();
        MejorarSalto();
        Disparar();
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

    void Saltar ()
    {
        if(nivel == 1) {
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                Debug.Log("Salto detectado");
                audioSource.PlayOneShot(audioJump);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                Debug.Log("Salto realizado");
            }
        }
        if (nivel == 2) {
            if (Input.GetKeyDown(KeyCode.W))
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

            if (nivel == 1)
            {
                bullet = Instantiate(bulletPrefab[0], firePoint.position + offset, Quaternion.identity);
                bulletScript = bullet.GetComponent<BulletScript>();
                bulletScript.SetDirection(facingRight ? Vector2.right : Vector2.left, bulletSpeed);
                bulletScript.SetScreenLimits(minX, maxX, minY, maxY); // <-- NUEVO
            }

            if (nivel == 2)
            {
                bullet = Instantiate(bulletPrefab[1], firePoint.position + offset, Quaternion.identity);
                bulletScript = bullet.GetComponent<BulletScript>();
                bulletScript.SetDirection(facingRight ? Vector2.right : Vector2.left, bulletSpeed);
                bulletScript.SetScreenLimits(minX, maxX, minY, maxY); // <-- NUEVO
            }
            
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
            nivel++;
            if (nivel < 2)
            {
                CambiarPrefab(nivel);
            }
            else
            {
                Debug.Log("No hay más niveles de prefab.");
            }
            Debug.Log("PowerUp recogido. Nivel actual: " + nivel);
        }
        else if (other.gameObject.CompareTag("ObstacleDamage"))
        {
            GameManager.Instance.RestarVida(1);  // o la cantidad de daño que quieras
            Debug.Log("Jugador golpeado por obstáculo dañino");
        }
    }

    void CambiarPrefab ( int nuevoNivel )
    {
        Vector3 posicionActual = transform.position;
        Quaternion rotacionActual = transform.rotation;

        Destroy(gameObject);
        Debug.Log("Cambiando prefab a nivel: " + nuevoNivel);

        Instantiate(playerPrefabs[nuevoNivel], posicionActual, rotacionActual);
    }


}
