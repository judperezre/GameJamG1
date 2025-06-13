using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JefeFinal : MonoBehaviour
{
    public float radioBuscar;
    public LayerMask capaJugador;
    public Transform transformJugador;
    public float velocidadMovimiento;
    public float distanciaMaxima;
    public Vector3 puntoInicial;
    public EstadosMovimientos estadoActual;
    [SerializeField] private int vida = 100;
    [SerializeField] private int daño;

    public static GameManager Instance;

    [Header("UIJefe")]
    public TextMeshProUGUI vidaJefeText;
    public GameObject panelJefe;


    public enum EstadosMovimientos
    {
        Esperando,
        Siguiendo,
        Volviendo
    }

    private void Start ()
    {
        puntoInicial = transform.position;
    }
    // Start is called before the first frame update

    private void Update ()
    {
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
            default:
                break;
        }
    }
    private void EstadoEsperando ()
    {
        Collider2D jugadorCollider = Physics2D.OverlapCircle(transform.position, radioBuscar, capaJugador);

        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;
        }
        estadoActual = EstadosMovimientos.Siguiendo;
    }

    private void EstadoSiguiendo ()
    {
        if (transformJugador == null)
        {
            estadoActual = EstadosMovimientos.Volviendo;
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, transformJugador.position, velocidadMovimiento * Time.deltaTime);
        if (Vector2.Distance(transform.position, puntoInicial) > distanciaMaxima || Vector2.Distance(transform.position, transformJugador.position) > distanciaMaxima)
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

    public void RecibirDaño ( int daño )
    {
        vida -= daño;
        if (vida <= 0)
        {
            Morir();
        }
        ActualizarUIJefe();
    }

    void Morir ()
    {
        Debug.Log("JEFE muerto");
        Destroy(gameObject);
        GameManager.Instance.WinGame();

    }


    private void ActualizarUIJefe ()
    {
        vidaJefeText.text = "Vida: " + vida.ToString();
    }


    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBuscar);
        Gizmos.DrawWireSphere(puntoInicial, distanciaMaxima);
    }
}
