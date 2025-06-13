using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Audio;

public class JefeFinal : MonoBehaviour
{
    public float radioBuscar;
    public LayerMask capaJugador;
    public Transform transformJugador;
    public float velocidadMovimiento;
    [SerializeField] private int vida = 100;
    [SerializeField] private int daño;




    /*Seccion Cambio*/
    public static GameManager Instance;

    [Header("UIJefe")]
    public TextMeshProUGUI vidaJefeText;
    public GameObject panelJefe;

    [Header("Valores del jefe")]
    public int vidaJefeInicial = 100;
    private int vidaJefeActual;

    public Vector3 puntoInicial;
    public float distanciaMaxima;
    public EstadosMovimiento estadoActual;
    public enum EstadosMovimiento
    {
        Esperando,
        Siguiendo,
        Volviendo,
    }

    private void Start ()
    {
        puntoInicial = transform.position;
    }


    //[Header("Limite de Pantalla")]
    //[SerializeField] private float minX, maxX, minY, maxY;
    // Start is called before the first frame update

    //void LateUpdate ()
    //{
    //    LimitarPosicion();
    //}
    //void LimitarPosicion ()
    //{
    //    Vector3 clampedPosition = transform.position;
    //    clampedPosition.x = Mathf.Clamp(transform.position.x, minX, maxX);
    //    clampedPosition.y = Mathf.Clamp(transform.position.y, minY, maxY);
    //    transform.position = clampedPosition;
    //}


    /*Seccion Cambio*/

    private void Update()
    {
        /*CAMBIO*/
        switch (estadoActual)
        {
            case EstadosMovimiento.Esperando:
                EstadoEsperando();
                break;
            case EstadosMovimiento.Siguiendo:
                EstadoSiguiendo();
                break;
            case EstadosMovimiento.Volviendo:
                break;
        }
        /*CAMBIO*/

    }

    private void EstadoEsperando ()
    {
        Collider2D jugadorCollider = Physics2D.OverlapCircle(transformJugador.position, radioBuscar, capaJugador);

        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;

            estadoActual = EstadosMovimiento.Siguiendo;
        }

    }


    private void EstadoSiguiendo ()
    {
        if(transformJugador == null)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            panelJefe.SetActive(false);
        }

        transform.position = Vector2.MoveTowards(transform.position, transformJugador.position, velocidadMovimiento * Time.deltaTime);

        if(Vector2.Distance(transform.position, puntoInicial) < distanciaMaxima || 
           Vector2.Distance(transform.position, transformJugador.position) > distanciaMaxima)
        {
            panelJefe.SetActive(true);
            estadoActual = EstadosMovimiento.Volviendo;
            transformJugador = null;
        }
    }



    public void RecibirDaño ( int daño )
    {
        vida -= daño;
        Debug.Log("JEFE RECIBIO -" + vida + " DE DAÑO");
        if (vida <= 0)
        {
            Morir();
        }
        ActualizarUIJefe();
    }


    private void ActualizarUIJefe ()
    {
        vidaJefeText.text = "Vida: " + vida.ToString();
    }


    void Morir ()
    {
        Debug.Log("JEFE muerto");
        Destroy(gameObject);
        GameManager.Instance.WinGame();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBuscar);
        Gizmos.DrawWireSphere(puntoInicial, distanciaMaxima);
    }

    private void OnCollisionEnter2D ( Collision2D collision )
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.RestarVida(daño);  // <-- Así accedemos correctamente
            Debug.Log("Jugador golpeado por Jefe");
        }
    }
}
