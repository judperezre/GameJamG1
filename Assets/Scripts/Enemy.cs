using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] float speed = 5.0f;
    [SerializeField] private int vida = 3;
    [SerializeField] private int daño;
    [SerializeField] private float radioBuscar;
    [SerializeField] private LayerMask capaJugador;
    [SerializeField] private GameObject projectilePrefab;

    private float distanciaMaxima;
    private Vector3 puntoInicial;
    private Rigidbody2D rb;
    private Transform target;
    private bool isDeath = false;

    public enum EstadosMovimientos { Esperando, Siguiendo, Volviendo }
    private EstadosMovimientos estadoActual;

    void Start ()
    {
        distanciaMaxima = radioBuscar * 1.5f;
        puntoInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        StartCoroutine(FireProjectile());
    }

    IEnumerator FireProjectile ()
    {
        while (!isDeath)
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3);
        }
    }

    void Update ()
    {
        BuscarTarget(); // Agregado para actualizar siempre el target si cambia de prefab

        switch (estadoActual)
        {
            case EstadosMovimientos.Esperando: EstadoEsperando(); break;
            case EstadosMovimientos.Siguiendo: EstadoSiguiendo(); break;
            case EstadosMovimientos.Volviendo: EstadoVolviendo(); break;
        }
    }

    void BuscarTarget ()
    {
        if (target == null && PlayerController.Instance != null)
        {
            target = PlayerController.Instance.transform;
        }
    }

    private void EstadoEsperando ()
    {
        Collider2D jugadorCollider = Physics2D.OverlapCircle(transform.position, radioBuscar, capaJugador);

        if (jugadorCollider)
        {
            target = jugadorCollider.transform;
            estadoActual = EstadosMovimientos.Siguiendo;
        }
    }

    private void EstadoSiguiendo ()
    {
        if (target == null)
        {
            estadoActual = EstadosMovimientos.Volviendo;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, puntoInicial) > distanciaMaxima || Vector2.Distance(transform.position, target.position) > distanciaMaxima)
        {
            estadoActual = EstadosMovimientos.Volviendo;
            target = null;
        }
    }

    private void EstadoVolviendo ()
    {
        transform.position = Vector2.MoveTowards(transform.position, puntoInicial, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, puntoInicial) < 0.1f)
        {
            estadoActual = EstadosMovimientos.Esperando;
        }
    }

    public void RecibirDaño ( int daño )
    {
        vida -= daño;
        if (vida <= 0)
        {
            Morir();
        }
    }

    void Morir ()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D ( Collision2D collision )
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.RestarVida(daño);
        }
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBuscar);
        Gizmos.DrawWireSphere(puntoInicial, distanciaMaxima);
    }
}
