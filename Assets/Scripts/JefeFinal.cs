using System.Collections;
using TMPro;
using UnityEngine;

public class JefeFinal : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float radioBuscar;
    [SerializeField] private LayerMask capaJugador;
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float distanciaMaxima;
    private Vector3 puntoInicial;
    private Transform transformJugador;
    private GameObject player;

    [Header("Vida y daño")]
    [SerializeField] private int vida = 100;
    [SerializeField] private int daño;

    [Header("UI Jefe")]
    [SerializeField] private TextMeshProUGUI vidaJefeText;
    [SerializeField] private GameObject panelJefe;

    public EstadosMovimientos estadoActual;

    public static GameManager Instance;

    public enum EstadosMovimientos
    {
        Esperando,
        Siguiendo,
        Volviendo
    }

    private void Start ()
    {
        puntoInicial = transform.position;
        distanciaMaxima = radioBuscar * 1.5f;

        // Buscamos al jugador al inicio
        BuscarJugador();
        ActualizarUIJefe();
    }

    private void Update ()
    {
        if (player == null) BuscarJugador();

        switch (estadoActual)
        {
            case EstadosMovimientos.Esperando:
                EstadoEsperando();
                break;
            case EstadosMovimientos.Siguiendo:
                EstadoSiguiendo();
                break;
            case EstadosMovimientos.Volviendo:
                EstadoVolviendo();
                break;
        }
    }

    private void BuscarJugador ()
    {
        if (PlayerController.Instance != null)
        {
            player = PlayerController.Instance.gameObject;
            transformJugador = player.transform;
        }
    }

    private void EstadoEsperando ()
    {
        // Si ya tenemos referencia al jugador, no buscamos nuevamente
        if (player != null)
        {
            float distanciaJugador = Vector2.Distance(transform.position, player.transform.position);
            if (distanciaJugador <= radioBuscar)
            {
                panelJefe.SetActive(true);
                transformJugador = player.transform;
                estadoActual = EstadosMovimientos.Siguiendo;
            }
        }
    }

    private void EstadoSiguiendo ()
    {
        if (transformJugador == null)
        {
            estadoActual = EstadosMovimientos.Volviendo;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, transformJugador.position, velocidadMovimiento * Time.deltaTime);

        if (Vector2.Distance(transform.position, puntoInicial) > distanciaMaxima
            || Vector2.Distance(transform.position, transformJugador.position) > distanciaMaxima)
        {
            estadoActual = EstadosMovimientos.Volviendo;
            transformJugador = null;
        }
    }

    private void EstadoVolviendo ()
    {
        transform.position = Vector2.MoveTowards(transform.position, puntoInicial, velocidadMovimiento * Time.deltaTime);

        if (Vector2.Distance(transform.position, puntoInicial) < 0.1f)
        {
            estadoActual = EstadosMovimientos.Esperando;
        }
    }

    public void RecibirDaño ( int dañoRecibido )
    {
        vida -= dañoRecibido;
        if (vida <= 0)
        {
            Morir();
        }
        ActualizarUIJefe();
    }

    private void Morir ()
    {
        Debug.Log("JEFE muerto");
        Destroy(gameObject);
        GameManager.Instance.WinGame();
    }

    private void ActualizarUIJefe ()
    {
        vidaJefeText.text = "Vida: " + vida.ToString();
    }

    private void OnCollisionEnter2D ( Collision2D collision )
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.RestarVida(daño);
            Debug.Log("Jugador golpeado por Jefe");
        }
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBuscar);
        Gizmos.DrawWireSphere(puntoInicial, distanciaMaxima);
    }
}
