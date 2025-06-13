using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JefeFinal : MonoBehaviour
{
    public float radioBuscar;
    public LayerMask capaJugador;
    public Transform transformJugador;
    public float velocidadMovimiento;
    // Start is called before the first frame update

    private void Update()
    {
        Collider2D jugadorCollider = Physics2D.OverlapCircle(transformJugador.position, radioBuscar, capaJugador);

        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;
        }
        transform.position = Vector2.MoveTowards(transform.position, transformJugador.position, velocidadMovimiento * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBuscar);
    }
}
