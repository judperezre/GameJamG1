using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // 1. Variables principales
    [SerializeField] private int daño = 1;
    [SerializeField] private bool dañoContinuo = false;
    [SerializeField] private float tiempoEntreDaños = 1.0f;

    private float timer = 0f;
    private bool jugadorDentro = false;

    // 2. Trigger enter
    private void OnTriggerEnter2D ( Collider2D other )
    {
        if (other.CompareTag("Player"))
        {
            // Daño instantáneo al entrar
            other.GetComponent<PlayerController>().RecibirDaño(daño);
            jugadorDentro = true;
            timer = tiempoEntreDaños; // resetea el timer para daño continuo
        }
    }

    // 3. Trigger exit
    private void OnTriggerExit2D ( Collider2D other )
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
        }
    }

    // 4. Update (solo si es daño continuo)
    private void Update ()
    {
        if (dañoContinuo && jugadorDentro)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerController>().RecibirDaño(daño);
                timer = tiempoEntreDaños; // reinicia el cooldown
            }
        }
    }

}
