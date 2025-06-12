using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] float speed = 5.0f;
    [SerializeField] private int vida = 3;
    [SerializeField] private int nivel;
    [SerializeField] private int daño;

    private Rigidbody2D enemyRb;
    private GameObject player;
    public GameObject projectilePrefab;
    private bool isDeath = false;

    //[Header("Patrulla")]
    //[SerializeField] private float velocidad = 2f;
    //[SerializeField] private Transform puntoA;
    //[SerializeField] private Transform puntoB;

    private Vector3 destinoActual;
    private Rigidbody2D rb;
    private bool haciaA = false;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        //destinoActual = puntoB.position;
        enemyRb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        StartCoroutine(FireProjectile());
    }


    // Update is called once per frame
    void Update ()
    {
        enemyRb.transform.Translate((player.transform.position - transform.position).normalized * speed * Time.deltaTime);
    }

    void FixedUpdate ()
    {
        //if(nivel == 1)
        //{
        //    Patrullar();
        //}
    }

    //void Patrullar ()
    //{
    //    Vector3 nuevaPosicion = Vector3.MoveTowards(transform.position, destinoActual, velocidad * Time.fixedDeltaTime);
    //    rb.MovePosition(nuevaPosicion);

    //    // Voltear el sprite según la dirección
    //    Vector3 direccion = destinoActual - transform.position;
    //    if (direccion.x != 0)
    //    {
    //        Vector3 escala = transform.localScale;
    //        escala.x = Mathf.Sign(direccion.x) * Mathf.Abs(escala.x);
    //        transform.localScale = escala;
    //    }

    //    if (Vector3.Distance(transform.position, destinoActual) < 0.1f)
    //    {
    //        haciaA = !haciaA;
    //        destinoActual = haciaA ? puntoA.position : puntoB.position;
    //    }
    //}

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
        Debug.Log("Enemigo muerto");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.RestarVida(daño);  // <-- Así accedemos correctamente
            Debug.Log("Jugador golpeado por enemigo");
        }
    }




    IEnumerator FireProjectile ()
    {
        while (!isDeath)
        {
            Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
            yield return new WaitForSeconds(3);

        }
    }

}
