using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int vida = 3;

    [Header("Patrulla")]
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;

    private Vector3 destinoActual;
    private Rigidbody2D rb;
    private bool haciaA = false;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        destinoActual = puntoB.position;
    }

    void FixedUpdate ()
    {
        Patrullar();
    }

    void Patrullar ()
    {
        Vector3 nuevaPosicion = Vector3.MoveTowards(transform.position, destinoActual, velocidad * Time.fixedDeltaTime);
        rb.MovePosition(nuevaPosicion);

        // Voltear el sprite según la dirección
        Vector3 direccion = destinoActual - transform.position;
        if (direccion.x != 0)
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Sign(direccion.x) * Mathf.Abs(escala.x);
            transform.localScale = escala;
        }

        if (Vector3.Distance(transform.position, destinoActual) < 0.1f)
        {
            haciaA = !haciaA;
            destinoActual = haciaA ? puntoA.position : puntoB.position;
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
}
