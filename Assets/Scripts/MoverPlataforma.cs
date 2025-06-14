using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverPlataforma : MonoBehaviour
{
    [Header("Patrulla")]
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;

    private Vector3 destinoActual;
    private bool haciaA = false;

    void Start ()
    {
        destinoActual = puntoB.position;
    }

    void Update ()
    {
        Patrullar();
    }

    void Patrullar ()
    {
        Vector3 nuevaPosicion = Vector3.MoveTowards(transform.position, destinoActual, velocidad * Time.deltaTime);
        transform.position = nuevaPosicion;  // Aquí eliminamos el Rigidbody

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
