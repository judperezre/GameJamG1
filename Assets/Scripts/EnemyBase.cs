using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] protected float radioBuscar;
    [SerializeField] protected LayerMask capaJugador;
    [SerializeField] protected float velocidadMovimiento;
    protected float distanciaMaxima;
    protected Vector3 puntoInicial;
    protected Transform transformJugador;
    protected GameObject player;

    [Header("Vida y daño")]
    [SerializeField] protected int vida = 10;
    [SerializeField] protected int daño = 1;

    public enum EstadosMovimientos { Esperando, Siguiendo, Volviendo }
    protected EstadosMovimientos estadoActual;

    protected virtual void Start ()
    {
        puntoInicial = transform.position;
        distanciaMaxima = radioBuscar * 1.5f;
        estadoActual = EstadosMovimientos.Esperando;
        BuscarJugador();
    }

    protected virtual void Update ()
    {
        BuscarJugador();

        switch (estadoActual)
        {
            case EstadosMovimientos.Esperando: EstadoEsperando(); break;
            case EstadosMovimientos.Siguiendo: EstadoSiguiendo(); break;
            case EstadosMovimientos.Volviendo: EstadoVolviendo(); break;
        }
    }

    protected void BuscarJugador ()
    {
        if (transformJugador == null && PlayerController.Instance != null)
        {
            player = PlayerController.Instance.gameObject;
            transformJugador = player.transform;
        }
    }

    protected virtual void EstadoEsperando ()
    {
        if (player != null)
        {
            float distanciaJugador = Vector2.Distance(transform.position, player.transform.position);
            if (distanciaJugador <= radioBuscar)
            {
                estadoActual = EstadosMovimientos.Siguiendo;
            }
        }
    }

    protected void EstadoSiguiendo ()
    {
        if (transformJugador == null)
        {
            estadoActual = EstadosMovimientos.Volviendo;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, transformJugador.position, velocidadMovimiento * Time.deltaTime);
        GirarHaciaTarget(transformJugador.position);

        if (Vector2.Distance(transform.position, puntoInicial) > distanciaMaxima
            || Vector2.Distance(transform.position, transformJugador.position) > distanciaMaxima)
        {
            estadoActual = EstadosMovimientos.Volviendo;
            transformJugador = null;
        }
    }

    protected void EstadoVolviendo ()
    {
        transform.position = Vector2.MoveTowards(transform.position, puntoInicial, velocidadMovimiento * Time.deltaTime);
        GirarHaciaTarget(puntoInicial);

        if (Vector2.Distance(transform.position, puntoInicial) < 0.1f)
        {
            estadoActual = EstadosMovimientos.Esperando;
        }
    }

    protected void GirarHaciaTarget ( Vector3 objetivo )
    {
        if (objetivo.x > transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public virtual void RecibirDaño ( int dañoRecibido )
    {
        vida -= dañoRecibido;
        if (vida <= 0) Morir();
    }

    protected abstract void Morir ();

    protected void OnCollisionEnter2D ( Collision2D collision )
    {
        if (collision.gameObject.CompareTag("Player"))
            GameManager.Instance.RestarVida(daño);
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBuscar);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(puntoInicial, radioBuscar * 1.5f);
    }
}
