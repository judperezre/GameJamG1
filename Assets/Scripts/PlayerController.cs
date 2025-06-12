using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int vida = 3;

    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Salto Mejorado")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Disparo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireCooldown = 0.3f;
    [SerializeField] private float offsetBullet = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip audioJump;
    [SerializeField] private AudioClip audioDisparo;

    [Header("Limite de Pantalla")]
    [SerializeField] private float minX, maxX, minY, maxY;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalInput;
    private bool facingRight = true;
    private float fireCooldownTimer = 0f;

    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
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
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            audioSource.PlayOneShot(audioJump);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Debug.Log("Salto realizado");
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
        if (Input.GetKeyDown(KeyCode.Space) && fireCooldownTimer <= 0f)
        {
            audioSource.PlayOneShot(audioDisparo);
            Vector3 offset = facingRight ? Vector3.right * offsetBullet : Vector3.left * offsetBullet;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position + offset, Quaternion.identity);
            bullet.GetComponent<BulletScript>().SetDirection(facingRight ? Vector2.right : Vector2.left, bulletSpeed);
            Debug.Log("Disparo realizado");
            fireCooldownTimer = fireCooldown;
        }
    }

    public void RecibirDaño ( int daño )
    {
        vida -= daño;
        Debug.Log("Vida restante: " + vida);
        if (vida <= 0)
        {
            Morir();
        }
    }

    void Morir ()
    {
        Debug.Log("Enemigo muerto");
        Destroy(gameObject);
    }

    // -------- AQUÍ VA LO NUEVO ---------

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
}
