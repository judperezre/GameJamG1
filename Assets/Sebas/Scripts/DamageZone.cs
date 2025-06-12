using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // 1. Variables principales
    [SerializeField] private int da�o = 1;
    [SerializeField] private bool da�oContinuo = false;
    [SerializeField] private float tiempoEntreDa�os = 1.0f;

    private float timer = 0f;
    private bool jugadorDentro = false;

    // 2. Trigger enter
    private void OnTriggerEnter2D ( Collider2D other )
    {
        if (other.CompareTag("Player"))
        {
            // Da�o instant�neo al entrar
            other.GetComponent<PlayerController>().RecibirDa�o(da�o);
            jugadorDentro = true;
            timer = tiempoEntreDa�os; // resetea el timer para da�o continuo
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

    // 4. Update (solo si es da�o continuo)
    private void Update ()
    {
        if (da�oContinuo && jugadorDentro)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerController>().RecibirDa�o(da�o);
                timer = tiempoEntreDa�os; // reinicia el cooldown
            }
        }
    }

}
