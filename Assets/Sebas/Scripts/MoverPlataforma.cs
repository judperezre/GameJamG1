using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverPlataforma : MonoBehaviour
{

    [Header("Patrulla")]
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;
    private Rigidbody2D rb;
    private Vector3 destinoActual;
    private bool haciaA = false;

    // Start is called before the first frame update
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        destinoActual = puntoB.position;

    }

    // Update is called once per frame
    void Update()
    {

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
}
