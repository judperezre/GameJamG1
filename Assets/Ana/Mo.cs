using UnityEngine;

public class Mo : MonoBehaviour 
{
    public float velocidad = 5f;
    public float fuerzaSalto = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movimiento horizontal
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movimientoHorizontal * velocidad, rb.velocity.y);

        // Salto
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
    }
}