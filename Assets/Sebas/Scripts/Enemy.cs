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


    private Rigidbody2D rb;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        enemyRb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        StartCoroutine(FireProjectile());
    }


    // Update is called once per frame
    void Update ()
    {
        enemyRb.transform.Translate((player.transform.position - transform.position).normalized * speed * Time.deltaTime);
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
        Debug.Log("Enemigo muerto");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D ( Collision2D collision )
    {
        if (collision.gameObject.CompareTag("Player"))
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
